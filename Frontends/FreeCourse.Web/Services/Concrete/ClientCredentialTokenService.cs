using FreeCourse.Web.Models;
using FreeCourse.Web.Services.Abstract;
using IdentityModel.AspNetCore.AccessTokenManagement;
using IdentityModel.Client;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace FreeCourse.Web.Services.Concrete//135
{
    public class ClientCredentialTokenService : IClientCredentialTokenService
    {
        private readonly ServiceApiSettings _serviceApiSettings;
        private readonly ClientSettings _clientSettings;
        private readonly HttpClient _httpClient;
        private readonly IClientAccessTokenCache _clientAccessTokenCache;//bunu kurdugumuz pakete ulasmak ıcın tanımladık IdentityModel.aspnet.core 3.0 

        public ClientCredentialTokenService(IOptions<ServiceApiSettings> serviceApiSettings, IOptions<ClientSettings> clientSettings, HttpClient httpClient, IClientAccessTokenCache clientAccessTokenCache)
        {
            _serviceApiSettings = serviceApiSettings.Value;//appsetıngs.json doyasından verıalacak IOptions<ServiceApiSettings> olarak tanımlıyor depesıngınjectıonda
            _clientSettings = clientSettings.Value;//bunlar appsetıngs.json doyasından verıalacak IOptions<ClientSettings>larak tanımlıyor depesıngınjectıonda
            _httpClient = httpClient;
            _clientAccessTokenCache = clientAccessTokenCache;
        }

        public async Task<string> GetTokenAsync()
        {
            var currentToken = await _clientAccessTokenCache.GetAsync("WebClientToken",null);//cashte beynde boyle bır tokn varmı
            if (currentToken != null)
            {
                return currentToken.AccessToken;//varsa token dön
            }

            // appsetıngs _serviceApiSettings tagı uzerınden baglantı lınkı al
            var disco = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = _serviceApiSettings.IdentityBaseUrl,
                Policy = new DiscoveryPolicy { RequireHttps = false },//projeler https s kısmını ıptal etmıstık burda teyıt edıyoruz
            });
            if (disco.IsError)
            {
                throw disco.Exception;
            }
            var clientCredentialsTokenRequest = new ClientCredentialsTokenRequest//appsetıngs _clientSettings tagı uzerınden client bılgılerı de ekle
            {
                ClientId = _clientSettings.WebClient.ClientId,
                ClientSecret = _clientSettings.WebClient.ClientSecret,
                Address=disco.TokenEndpoint               // gıdıelek lınk bellı 
            };
            //hersey hazır. lınke git sorgula yenı token
            var newToken=await _httpClient.RequestClientCredentialsTokenAsync(clientCredentialsTokenRequest);
            if (newToken.IsError)
            {
                throw newToken.Exception; //tokende hata varsa fırlat sebebını
            }
            //token alındıysa hata yoks demek,cashe gönder suresıyle bırlıkte
            await _clientAccessTokenCache.SetAsync("WebClientToken", newToken.AccessToken, newToken.ExpiresIn,null);//token hafıaz al defaul suresı ıle

            return newToken.AccessToken;//token döndur


        }
    }
}
//tokenı memory kayıt edecez bunun ıcın IdentityModel.aspnet.core 3.0 paketı eklenıyor