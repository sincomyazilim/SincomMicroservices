using FreeCourse.Web.Models;
using FreeCourse.Web.Models.Identit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FreeCourse.Web.Services.Abstract//123
{
    public interface IUserService
    {
        Task<UserViewModel> GetUser();

        
    }
}
