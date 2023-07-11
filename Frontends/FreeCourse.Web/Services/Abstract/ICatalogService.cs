using FreeCourse.Web.Models.Catalog;
using FreeCourse.Web.Models.DiscountCourse;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FreeCourse.Web.Services.Abstract//130
{
    public interface ICatalogService
    {
        Task<List<GetCourseViewModel>> GetAllCourseAsync();
        Task<List<GetCategoryViewModel>> GetAllCategoryAsync();

        Task<List<GetCourseViewModel>> GetAllCourseByUserIdAsync(string userId);//bır kulllanıcın sahıp oldugu kurslar       
        Task<GetCourseViewModel>GetByCourseId(string courseId);//id ye göre kursu getır

        Task<bool> CreateCourseAsync(CreateCourseInput createCourseInput);
        Task<bool> UpdateCourseAsync(UpdateCourseInput updateCourseInput);
        Task<bool> DeleteCourseAsync(string courseId);

    }
}
