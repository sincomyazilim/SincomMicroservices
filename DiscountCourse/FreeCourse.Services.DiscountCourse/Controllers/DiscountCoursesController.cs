using FreeCourse.Services.DiscountCourse.Services.Abstract;
using FreeCourse.Shared.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using FreeCourse.Shared.ControlerBase;

namespace FreeCourse.Services.DiscountCourse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountCoursesController : CustomBaseController
    {
        private readonly ISharedIdentityService _sharedIdentityService;
        private readonly IDiscountCourseService _discountCourseService;


        public DiscountCoursesController(ISharedIdentityService sharedIdentityService, IDiscountCourseService discountCourseService)
        {
            _sharedIdentityService = sharedIdentityService;
            _discountCourseService = discountCourseService;
        }
        //--------------------------------------------------------------------

        [HttpGet]
        [Route("/api/[controller]/[action]/{code}")]

        public async Task<IActionResult> GetByCodeForCourse(string code, string courseId)
        {
            
            var userId = _sharedIdentityService.GetUserId;
            var discount = await _discountCourseService.GetByCodeAndUserIdAndCourseId(code, userId, courseId);
            return CreateActionResultInstance(discount);

        }
    }
}
