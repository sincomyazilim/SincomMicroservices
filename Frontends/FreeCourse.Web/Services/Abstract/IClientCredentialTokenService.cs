using System.Threading.Tasks;

namespace FreeCourse.Web.Services.Abstract
{
    public interface IClientCredentialTokenService//135
    {
        Task<string> GetTokenAsync();
    }
}
