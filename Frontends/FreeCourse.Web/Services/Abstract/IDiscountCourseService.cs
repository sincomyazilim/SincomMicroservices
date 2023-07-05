using FreeCourse.Web.Models.DiscountCourse;
using System.Threading.Tasks;

namespace FreeCourse.Web.Services.Abstract
{
    public interface IDiscountCourseService
    { 
        Task<DiscountCourseViewModel> GetDiscountCourse(DiscounCoursetApplyInputCodeAndCourseId model);
    }
}
