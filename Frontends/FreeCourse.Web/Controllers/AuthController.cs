using FreeCourse.Web.Models;
using FreeCourse.Web.Services.Abstract;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Principal;
using System.Threading.Tasks;

namespace FreeCourse.Web.Controllers//118-119
{
    public class AuthController : Controller
    {
        private readonly IIdentityService _identityService;//119

        public AuthController(IIdentityService identityService)
        {
            _identityService = identityService;
        }
        //--------------------------------------------------
        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInput signInput)//119
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var response = await _identityService.SignIn(signInput);
            if (!response.IsSuccessful)
            {
                response.Errors.ForEach(x =>
                {
                    ModelState.AddModelError(string.Empty, x);//login yaparken hata olursa kullanıcıadındamı sıfredemı hata oldugunu anlamasınlar dıye Empty işaretkedik
                });
                return View();
            }
            return RedirectToAction("Index", "Home");// veya return RedirectToAction("nameof(Index), "Home")
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);//startuo ta eklmeısık bu semayı sımdı burda cıkıs yap 
            await _identityService.RevokeRefreshToken();//burdada sılıyoruz
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}
