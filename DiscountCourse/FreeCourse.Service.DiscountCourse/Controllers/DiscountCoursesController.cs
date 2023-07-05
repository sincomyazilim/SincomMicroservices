using Microsoft.AspNetCore.Mvc;

namespace FreeCourse.Service.DiscountCourse.Controllers
{
    public class DiscountCoursesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

    }
}
