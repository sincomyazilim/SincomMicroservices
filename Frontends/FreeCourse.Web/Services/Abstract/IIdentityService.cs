using FreeCourse.Shared.Dtos;
using FreeCourse.Web.Models;
using IdentityModel.Client;
using System.Threading.Tasks;

namespace FreeCourse.Web.Services.Abstract//114
{
    public interface IIdentityService
    {
        Task<ResponseDto<bool>> SignIn(SignInput signInput);
        Task<TokenResponse> GetAccessTokenByRefreshToken();//token uzatmak ıcın 
        Task RevokeRefreshToken();//çıkıs yapıldıgında token bıtırecek
    }
}
