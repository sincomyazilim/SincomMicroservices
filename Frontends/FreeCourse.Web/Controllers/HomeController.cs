using FreeCourse.Shared.Services.Abstract;
using FreeCourse.Web.Exceptions;
using FreeCourse.Web.Models;
using FreeCourse.Web.Models.DiscountCourse;
using FreeCourse.Web.Services.Abstract;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCourse.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICatalogService _catalogService;//148
      private readonly IEgitmenAdiGetirmek _gitmenAdiGetirmek;
      


        public HomeController(ILogger<HomeController> logger, ICatalogService catalogService, IEgitmenAdiGetirmek gitmenAdiGetirmek  )
        {
            _logger = logger;
            _catalogService = catalogService;
            _gitmenAdiGetirmek = gitmenAdiGetirmek;
          
        }
        //------------------------------------------------
        public async Task<IActionResult> Index()
        {

            var users = await _gitmenAdiGetirmek.GetAllUser();
            TempData["userList"] = users;
            return View(await _catalogService.GetAllCourseAsync());
        }

        public async Task<IActionResult>Detail(string id)
        {
            var users = await _gitmenAdiGetirmek.GetAllUser();
            TempData["userList"] = users;
            var detailCourse =await _catalogService.GetByCourseId(id);
            return View(detailCourse);
        }

        //public async Task<IActionResult> ApplyDiscountCourse(DiscounCoursetApplyInputCodeAndCourseId model)
        //{
        //    if (!ModelState.IsValid)//167 kupon gırılmezse verılecek hata 
        //    {
        //        TempData["discountError"] = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).First();
        //        return RedirectToAction(nameof(Index));
        //    }

        //    var model2=new DiscountCourseViewModelInput { CourseId=model.CourseId,  Code = model.Code };

        //    var discountStatus = await _catalogService.ApplyDiscountCourse(model2);
        //    TempData["discountStatus"] = discountStatus;
        //    return RedirectToAction(nameof(Index));
        //}

        public IActionResult Privacy()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {    //150 error düzenleme
            var errorFeature=HttpContext.Features.Get<IExceptionHandlerFeature>();
            if (errorFeature != null&& errorFeature.Error is UnAuthorizeException)
            { 
                return RedirectToAction(nameof(AuthController.LogOut),"Auth");
            }
            //150 hata sınıfımız oldugu zaman cıkıs yaptırıp login  sayfasıan yönlenıyor


            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
