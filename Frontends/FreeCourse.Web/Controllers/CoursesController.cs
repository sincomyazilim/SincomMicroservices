using FreeCourse.Shared.Dtos;
using FreeCourse.Shared.Services.Abstract;
using FreeCourse.Web.Helpers;
using FreeCourse.Web.Models.Catalog;
using FreeCourse.Web.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace FreeCourse.Web.Controllers//134
{
    [Authorize]//134 login olmadan ulasılmaz
    public class CoursesController : Controller
    {
        private readonly ICatalogService _catalogService;
        private readonly ISharedIdentityService _sharedIdentityService;
        private readonly PhotoHelper _photoHelper;

        public CoursesController(ICatalogService catalogService, ISharedIdentityService sharedIdentityService = null, PhotoHelper photoHelper = null)
        {
            _catalogService = catalogService;
            _sharedIdentityService = sharedIdentityService;//identiyserver baglanıp userId getırmekıcın yazmıstık sahre katmanında 
            _photoHelper = photoHelper;
        }
        //-------------------------------------------------------------
        public async Task<IActionResult> GetUserAllCourses()//134
        {
            var userId = _sharedIdentityService.GetUserId;
            var userCourses = await _catalogService.GetAllCourseByUserIdAsync(userId);
            return View(userCourses);
        }
        [HttpGet]
        public async Task<IActionResult> CreateCourse() //138
        {
            var categories = await _catalogService.GetAllCategoryAsync();//kategırlerı getırdık
            ViewBag.categoryList = new SelectList(categories,"Id","Name");

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateCourse(CreateCourseInput createCourse)//139
        {
            var categories = await _catalogService.GetAllCategoryAsync();//kategırlerı getırdık
            ViewBag.categoryList = new SelectList(categories, "Id", "Name");
            if (!ModelState.IsValid) 
            {
                return View();
            }
            createCourse.UserId = _sharedIdentityService.GetUserId;//userId getırıyor sharedteko metot
             await _catalogService.CreateCourseAsync(createCourse); 
            return RedirectToAction(nameof(GetUserAllCourses));
        }
        [HttpGet]
        public async Task<IActionResult>UpdateCourse(string id)//140
        {
            var course = await _catalogService.GetByCourseId(id);//bu id li kategorilerı getırdık
            ViewBag.VarOlanResim = _photoHelper.GetPhotoStockUrl(course.Picture);
            var categories = await _catalogService.GetAllCategoryAsync();//kategırlerı getırdık
            
            
            if (course==null)
            {
                RedirectToAction(nameof(GetUserAllCourses));
            }
            ViewBag.categoryList = new SelectList(categories, "Id", "Name",course.Id);
          
            UpdateCourseInput updateCourseInput = new()
            {
                Id = course.Id,
                Name = course.Name,
                Picture = course.Picture,
                Price = course.Price,
                Description = course.Description,
                //Feature = new GetFeatureViewModel { Duration = course.Feature.Duration },
                Feature = course.Feature,
                CategoryId=course.CategoryId,
                UserId=course.UserId
            };
           return View(updateCourseInput);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCourse(UpdateCourseInput updateCourseInput)//141
        {
            var categories = await _catalogService.GetAllCategoryAsync();//kategırlerı getırd
            ViewBag.categoryList = new SelectList(categories, "Id", "Name", updateCourseInput.Id);
            if (!ModelState.IsValid)
            {
                return View();
            }
            await _catalogService.UpdateCourseAsync(updateCourseInput);
            return RedirectToAction(nameof(GetUserAllCourses));
        }
       
        
        
        public async Task<IActionResult>DeleteCourse(string id)//142
        {
            await _catalogService.DeleteCourseAsync(id);
            return RedirectToAction(nameof(GetUserAllCourses));
        }

    }
}
