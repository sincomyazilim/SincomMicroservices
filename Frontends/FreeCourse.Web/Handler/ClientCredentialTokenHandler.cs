using FreeCourse.Web.Exceptions;
using FreeCourse.Web.Services.Abstract;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace FreeCourse.Web.Handler//136
{
    public class ClientCredentialTokenHandler:DelegatingHandler
    {
        private readonly IClientCredentialTokenService _clientCredentialTokenService;

        public ClientCredentialTokenHandler(IClientCredentialTokenService clientCredentialTokenService)
        {
            _clientCredentialTokenService = clientCredentialTokenService;
        }
        //---------------------------------------------------------------
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            //var token = await _clientCredentialTokenService.GetTokenAsync();//token getor
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await _clientCredentialTokenService.GetTokenAsync());//gelen tokan header sakla

            var response=await base.SendAsync(request, cancellationToken);//gönder token hafıda
            if (response.StatusCode==System.Net.HttpStatusCode.Unauthorized)//hata varsa
            {
                throw new UnAuthorizeException();//kendı hatamızı fıtlasın
            }

            return response;//döndur repsonseyı
        }
    }
}
