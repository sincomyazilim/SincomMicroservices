using FreeCourse.Services.Catalog.Dtos.CoursesDto;
using FreeCourse.Shared.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FreeCourse.Services.Catalog.Services.Abstract//25
{
    public interface ICourseService
    {
        Task<ResponseDto<List<CourseDto>>> GetAllAsync();
        Task<ResponseDto<CourseDto>> GetByIdAsync(string id);
        Task<ResponseDto<List<CourseDto>>> GetAllByUserIdAsync(string userId);
        Task<ResponseDto<CourseDto>> CreateCourseAsync(CourseCreateDto courseCreateDto);
        Task<ResponseDto<NoContent>> UpdateCourseAsync(CourseUpdateDto courseUpdateDto);
        Task<ResponseDto<NoContent>> DeleteCourseAsync(string id);
    }
}
