using FreeCourse.Web.Exceptions;
using FreeCourse.Web.Services.Abstract;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace FreeCourse.Web.Handler//125
{
    public class ResourceOwnerPasswordTokenHandler:DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IIdentityService _identityService;
        private readonly ILogger<ResourceOwnerPasswordTokenHandler> _logger;

        public ResourceOwnerPasswordTokenHandler(IHttpContextAccessor httpContextAccessor, IIdentityService identityService, ILogger<ResourceOwnerPasswordTokenHandler> logger = null)
        {
            _httpContextAccessor = httpContextAccessor;
            _identityService = identityService;
            _logger = logger;
        }
        //---------------------------------------------------------------

        //DelegatingHandler bundan mıras alıyor ve bunun asagıdakı meduunu overıde ıle ezıyorz
        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            //token al
            var accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);

            //istegın hedaerın eklıyoruz
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            var responseResult=await base.SendAsync(request, cancellationToken);//istek gıttık
            if (responseResult.StatusCode==System.Net.HttpStatusCode.Unauthorized)//eger hata varsa uyusmuyorsa
            {
                var tokenResponse = await _identityService.GetAccessTokenByRefreshToken();//gel token refresle yıne token al
                if (tokenResponse!=null)//hata yoksa bos donmedıyse yenı token
                {   // yıne yukarda olsugu gıbı baslıga ekle bu yenı aldıgın tokenı
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenResponse.AccessToken);
                    
                    //Yıne ıstekyap
                   responseResult = await base.SendAsync(request, cancellationToken);//istek gıttık
                }
            }
            if (responseResult.StatusCode==System.Net.HttpStatusCode.Unauthorized)//istek gıtı ve hala uyusmuyorsa
            {
                //hata fırlatacaz ona gre yonedırme yaacak burdan  return RedirectToAction yapamıyoruz 

                throw new UnAuthorizeException();//bızım hatasınıfını dondurecek bızde bakacazbızımkıs ıse lofınsayfasına yönlendırecez

            }
            return responseResult;

        }
    }
}


//ıdendtıryservere her ıstek gıttıgınde o ıstegıngın baslagına headerına yanı token ekleneıp gonderılıyor bu sınıf bu tokenı eklemekısmını yapıyor
//ıstedıgımızde kullanlcak


//bu sınıf dalegettır bunu statupta gıdıp tanımayacaz.. cookıdekı bılgılerı alıyor bu sınıf