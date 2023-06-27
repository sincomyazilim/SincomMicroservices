using FreeCourse.Web.Models.Identit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FreeCourse.Web.Services.Abstract
{
    public interface IEgitmenAdiGetirmek
    {
        Task<List<UsersViewModel>> GetAllUser();
    }
}
