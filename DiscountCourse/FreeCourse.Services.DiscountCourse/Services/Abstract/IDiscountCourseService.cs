using FreeCourse.Shared.Dtos;
using System.Threading.Tasks;

namespace FreeCourse.Services.DiscountCourse.Services.Abstract
{
    public interface IDiscountCourseService
    {
        Task<ResponseDto<Models.DiscountCourse>>GetByCodeAndUserIdAndCourseId(string code, string userId,string courseId);//bu kullanıcının  bu code ıle ındımı olmusmu
    }
}
