using FreeCourse.Shared.Dtos;
using FreeCourse.Web.Models;
using FreeCourse.Web.Services.Abstract;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace FreeCourse.Web.Services.Concrete//117
{
    public class IdentityService : IIdentityService
    {
        private readonly HttpClient _httpClient;//bu microservıse baglantı kurmakıcınıdr 
        private readonly IHttpContextAccessor _httpContextAccessor;//cookıey ulasmak ııcn
        private readonly ClientSettings _clientSettings;//bunlar uzerınden appsetındekı verılere ulasıyoruz
        private readonly ServiceApiSettings _serviceApiSettings;//bunlar uzerınden appsetındekı verılere ulasıyoruz

        public IdentityService(HttpClient client, IHttpContextAccessor httpContextAccessor, IOptions<ClientSettings> clientSettings, IOptions<ServiceApiSettings> serviceApiSettings)
        {
            _httpClient = client;
            _httpContextAccessor = httpContextAccessor;
            _clientSettings = clientSettings.Value;//degererını appsetıngs uzerınde alıyor
            _serviceApiSettings = serviceApiSettings.Value;//degererını appsetıngs uzerınde alıyor
        }
        //-------------------------------------------------------------------------------------------
        public async Task<TokenResponse> GetAccessTokenByRefreshToken()//121
        {
            // adres lınkı ayar bu lınkte http s skısmını pasıt et false et yanı
            var disco = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = _serviceApiSettings.IdentityBaseUrl,
                Policy = new DiscoveryPolicy { RequireHttps = false },//projeler https s kısmını ıptal etmıstık burda teyıt edıyoruz
            });
            if (disco.IsError)
            {
                throw disco.Exception;
            }
            //refreshtoken olustur,var olan token alındı
            var refreshToken = await _httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);
            //bılgılerı oldur
            RefreshTokenRequest refreshTokenRequest = new RefreshTokenRequest{//bu metotu dolduruyorz
                ClientId=_clientSettings.WebClientForUser.ClientId,
                ClientSecret=_clientSettings.WebClientForUser.ClientSecret,
                RefreshToken = refreshToken,//gerı getırdıgımız tokena göre doldur
                Address=disco.TokenEndpoint
            };
            //istek gıttı
            var token=await _httpClient.RequestRefreshTokenAsync(refreshTokenRequest);//istek gıttı
            if (token.IsError)
            {
                return null;//hata varsa sımdılık bos dön
            }//elimizde zaten hazır dolu token var bız sadece suresını uzatacaz

            // hazır gelen token suresını uzatır
           var  authenticationTokens=new List<AuthenticationToken>()//bu metot hazır olan token bılgılerıne gore doldurur
            {
                new AuthenticationToken{Name=OpenIdConnectParameterNames.AccessToken,Value=token.AccessToken},//doluyor
                new AuthenticationToken{Name=OpenIdConnectParameterNames.RefreshToken,Value=token.RefreshToken},//doluyor
                new AuthenticationToken{Name=OpenIdConnectParameterNames.ExpiresIn,Value=DateTime.Now.AddSeconds(token.ExpiresIn).ToString("o",CultureInfo.InvariantCulture)},//doluyor
            };//sure uzattık

            var authenticationResult = await _httpContextAccessor.HttpContext.AuthenticateAsync();// gıdıp sonucu getıro 
            
            var properties=authenticationResult.Properties;//sonucu ozellık degıskenıe atatık
            properties.StoreTokens(authenticationTokens);//ozellık degıskene var olan tokeni atatık

            //cookeı ekledık tokenı
            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, authenticationResult.Principal, properties);
            return token;//token gerı douyor lazım olabılırdıye

        }

        public async Task RevokeRefreshToken()//122
        {
            // adres lınkı ayar bu lınkte http s skısmını pasıt et false et yanı
            var disco = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = _serviceApiSettings.IdentityBaseUrl,
                Policy = new DiscoveryPolicy { RequireHttps = false },//projeler https s kısmını ıptal etmıstık burda teyıt edıyoruz
            });
            if (disco.IsError)
            {
                throw disco.Exception;
            }
            //refleme yonetımyle tokanı cektık
            var refreshToken = await _httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);

            TokenRevocationRequest tokenRevocationRequest = new()//token iptal etmek ıcın bılgıler metoda yuklenıyor
            {
                ClientId=_clientSettings.WebClientForUser.ClientId,
                ClientSecret=_clientSettings.WebClientForUser.ClientSecret,
                Address=disco.RevocationEndpoint,
                Token=refreshToken,
                TokenTypeHint="refresh_token"
            };
            await _httpClient.RevokeTokenAsync(tokenRevocationRequest);//tokenı burdada sılıyoruz


        }

        public async Task<ResponseDto<bool>> SignIn(SignInput signInput)
        {
            
            // adres lınkı ayar bu lınkte http s skısmını pasıt et false et yanı
            var disco = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = _serviceApiSettings.IdentityBaseUrl,
                Policy = new DiscoveryPolicy { RequireHttps = false },//projeler https s kısmını ıptal etmıstık burda teyıt edıyoruz
            });
            if (disco.IsError)
            {
                throw disco.Exception;
            }
            // password 
            var passwordtokenRequest = new PasswordTokenRequest
            {
                ClientId = _clientSettings.WebClientForUser.ClientId,
                ClientSecret = _clientSettings.WebClientForUser.ClientSecret,
                UserName = signInput.Email,
                Password = signInput.Password,
                Address = disco.TokenEndpoint//istek yapacak ades lınk yani
            };
            //token al
            var token = await _httpClient.RequestPasswordTokenAsync(passwordtokenRequest);//istek gıttı
            //hata varsa hatayı göster
            if (token.IsError)
            {
                var responseContext = await token.HttpResponse.Content.ReadAsStringAsync();
                var errorDto = JsonSerializer.Deserialize<ErrorDto>(responseContext, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });//hta donerse ıcını oku ama jsona gore cevırıken kucuk buyuk harf dıyarlıdır,sen dıkkat etme
                return ResponseDto<bool>.Fail(errorDto.Errors, 400);//hata döndur
            }
            //hata yok.. token elımızdedır.bu token ıcınde kullanıcı yan bılgısı yok sımdı onları ekleyecez

            var userInfiRequest = new UserInfoRequest//kullanıcı bılgılerı ııcn degısken tanımladık
            {
                Token = token.AccessToken,//setledık
                Address = disco.UserInfoEndpoint,//setledık}
            };

            var userInfo = await _httpClient.GetUserInfoAsync(userInfiRequest);//istek yaptık
            if (userInfo.IsError)//hata varsa hata fırlat
            {
                throw userInfo.Exception;
            }

            //userInfo elımızde var,bunu claimIdentıy cevırıyoruz
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(userInfo.Claims,                                                     CookieAuthenticationDefaults.AuthenticationScheme,"name","role");//name  ve role viewler kullanılmak uzere sımdıden ayarlarıuz eklıyoruz

            //cookı temellerını olustruyrouz---> artık cookı hazır 
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);


            var authenticationProperties = new AuthenticationProperties();
            authenticationProperties.StoreTokens(new List<AuthenticationToken>()
            {
                new AuthenticationToken{Name=OpenIdConnectParameterNames.AccessToken,Value=token.AccessToken},
                new AuthenticationToken{Name=OpenIdConnectParameterNames.RefreshToken,Value=token.RefreshToken},
                new AuthenticationToken{Name=OpenIdConnectParameterNames.ExpiresIn,Value=DateTime.Now.AddSeconds(token.ExpiresIn).ToString("o",CultureInfo.InvariantCulture)},
            });
            authenticationProperties.IsPersistent = signInput.IsRemember;//beni hatırla

            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, authenticationProperties);

            return ResponseDto<bool>.Success(200);
        }
    }
}
//SignIn metodu ıle kullancı emaıl ve sıfre bılgısını gırdıkten sonra ıdentıtserver ıstek atıp kullancıların ve yetkılerının ne oldugu hakkında bılgıyle token olsturuyor ve kullancı bılgılerını ekledıktıkten sonra cokıeye eklenıyor 60 gunlul suresı var cıkıs yapıladıgı surece.60 gunde bır refresh token metodu ıle sre uzatılır