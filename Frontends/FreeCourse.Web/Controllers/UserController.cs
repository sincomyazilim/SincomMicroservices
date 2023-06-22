using FreeCourse.Web.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FreeCourse.Web.Controllers
{
    [Authorize]//126 bunu bu kısma ekledıgımızdebun abgalı butun endoıntler yanı mettolar logın olmadan gorunmaz
    public class UserController : Controller//126
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        //--------------------------------------------------
        public async Task<IActionResult> Index()
        {
          
            return View(await _userService.GetUser());
        }
    }
}
