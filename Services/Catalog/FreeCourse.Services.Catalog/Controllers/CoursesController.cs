
using FreeCourse.Services.Catalog.Dtos.CoursesDto;
using FreeCourse.Services.Catalog.Services.Abstract;
using FreeCourse.Shared.ControlerBase;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FreeCourse.Services.Catalog.Controllers//26 videoda shared katamnını asagıdakı kodu refrasn ekledıkkı framwork gorebılsın lıbray oldugu ıcın görmuyor
                                                 //<ItemGroup>  <FrameworkReference Include="Microsoft.AspNetCore.App"/>  </ItemGroup>
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : CustomBaseController   //ControllerBase bunu degıstırdık shrade projesıdekı CustomBaseController kendı sınıfımız ekledık hata kodunu rahat dönebıleım dıye
    {
        private readonly ICourseService _courseService;

        public CoursesController(ICourseService courseService)
        {
            _courseService = courseService;
        }
        //-----------------------------------------------------
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _courseService.GetAllAsync();
            return CreateActionResultInstance(response);
        }
       
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var response=await _courseService.GetByIdAsync(id);
            return CreateActionResultInstance(response);//kendı sınıfımız yazdık shared katmanındakı sınıftır
        }

        [HttpGet]
        [Route("/api/[controller]/GetAllByUserId/{userId}")]
        public async Task<IActionResult> GetAllByUserId(string userId)
        {
            var response = await _courseService.GetAllByUserIdAsync(userId);

            return CreateActionResultInstance(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CourseCreateDto courseCreateDto)
        {
            var response = await _courseService.CreateCourseAsync(courseCreateDto);
            return CreateActionResultInstance(response);
        }

        [HttpPut]
        public async Task<IActionResult> Update(CourseUpdateDto courseUpdateDto)
        {
            var response = await _courseService.UpdateCourseAsync(courseUpdateDto);
            return CreateActionResultInstance(response);
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var response = await _courseService.DeleteCourseAsync(id);
            return CreateActionResultInstance(response);//kendı sınıfımız yazdık shared katmanındakı sınıftır
        }

    }
}
