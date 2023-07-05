using IdentityServer4.Validation;
using System.CodeDom.Compiler;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCourse.IdentityServer.Services//193
{
    public class TokenExchangeExtensionGrantValidator : IExtensionGrantValidator
    {
        public string GrantType => "urn:ietf:params:oauth:grant-type:token-exchange";

        private readonly ITokenValidator _tokenValidator;

        public TokenExchangeExtensionGrantValidator(ITokenValidator tokenValidator)
        {
            _tokenValidator = tokenValidator;
        }

        public async Task ValidateAsync(ExtensionGrantValidationContext context)
        {
            var requestRaw = context.Request.Raw.ToString();

            var token = context.Request.Raw.Get("subject_token");

            if (string.IsNullOrEmpty(token))
            {
                context.Result = new GrantValidationResult(IdentityServer4.Models.TokenRequestErrors.InvalidRequest, "token EKSİK");
                return;
            }

            var tokenValidateResult = await _tokenValidator.ValidateAccessTokenAsync(token);

            if (tokenValidateResult.IsError)
            {
                context.Result = new GrantValidationResult(IdentityServer4.Models.TokenRequestErrors.InvalidGrant, "token YANLIŞ");

                return;
            }

            var subjectClaim = tokenValidateResult.Claims.FirstOrDefault(c => c.Type == "sub");

            if (subjectClaim == null)
            {
                context.Result = new GrantValidationResult(IdentityServer4.Models.TokenRequestErrors.InvalidGrant, "token İÇİNDE KULLANICI BILGISI OLMASI ŞART");

                return;
            }

            context.Result = new GrantValidationResult(subjectClaim.Value, "access_token", tokenValidateResult.Claims);

            return;
        }
    }
}


//bu sınıf prohjenızde kullanılan ve dısardan erısım olmasını ıstemedıgınız ve proje ıcı kendı aralarında kullanılan mıcroservısler varsa
//Genel token ıle degılde kendı ıcnlerıdnee var olan token degıstırerek yenı token ıle o mıcroservıslere ulasmayı hedefler geneşl bılgı 192 hoca anlatıyor... bu POROJEDE BEN SADECE EKLEYECEM AMA ILGILI FAKEPAMEYMENT VE DISCOUNT MICROSERVISLERINE EKLEMEYECEM