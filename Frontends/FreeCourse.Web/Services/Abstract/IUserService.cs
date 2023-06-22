using FreeCourse.Web.Models;
using System.Threading.Tasks;

namespace FreeCourse.Web.Services.Abstract//123
{
    public interface IUserService
    {
        Task<UserViewModel> GetUser();
    }
}
