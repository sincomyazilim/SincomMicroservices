﻿using FreeCourse.Web.Models.Basket;
using FreeCourse.Web.Models.Discount;
using FreeCourse.Web.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCourse.Web.Controllers//156
{
    [Authorize]//giriş sartı yapıyoruz
    public class BasketController : Controller
    {
        private readonly ICatalogService _catalogService;
        private readonly IBasketService _basketService;

        public BasketController(ICatalogService catalogService, IBasketService basketService)
        {
            _catalogService = catalogService;
            _basketService = basketService;
        }
        //-----------------------------------------------
        public async Task<IActionResult> Index()
        {
            var basket = await _basketService.GetBasket();
            return View(basket);
        }

        public async Task<IActionResult> AddBasketItem(string courseId)
        {
            var course = await _catalogService.GetByCourseId(courseId);//kursu secttık kurs bılgısı elımde
            var basketItem = new BasketItemViewModel //basketItemmodel olsutrudk gelen bılgılere doldurduk
            {
                CourseId = course.Id,
                CourseName = course.Name,
                Price = course.Price

            };
            await _basketService.AddBasketItem(basketItem);//ekle

            return RedirectToAction(nameof(Index));//sepete yönledır
        }

        public async Task<IActionResult> RemoveBasketItem(string courseId)//basketıtemlerını siler
        {
            await _basketService.RemoveBasketItem(courseId);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> ApplyDiscount(DiscountApplyInputCode discountApplyInputCode)//165
        {
            if (!ModelState.IsValid)//167 kupon gırılmezse verılecek hata 
            {
                TempData["discountError"]=ModelState.Values.SelectMany(x=>x.Errors).Select(x=>x.ErrorMessage).First();
                return RedirectToAction(nameof(Index));
            }

            var discountStatus = await _basketService.ApplyDiscount(discountApplyInputCode.Code);//bu metotda dırk seepet guncelıyor 
            TempData["discountStatus"] = discountStatus;//budurumu temdata saklıyoruzkı hafızada kalsın kı baskyerd e ıptal etmek ıcın kullanabılılelm
            return RedirectToAction(nameof(Index));
        }


        ////tek course göre ındırım
        //public async Task<IActionResult> ApplyDiscountForCourse(DiscountApplyInputCodeAndCourseIdControl discountApplyInputCodeAndCourseIdControl)
        //{
        //    if (!ModelState.IsValid)//167 kupon gırılmezse verılecek hata 
        //    {
        //        TempData["discountError"] = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).First();
        //        return RedirectToAction(nameof(Index));
        //    }

        //    var discountStatus = await _basketService.ApplyDiscountForCourse(discountApplyInputCodeAndCourseIdControl.Code, discountApplyInputCodeAndCourseIdControl.CourseId);//bu metotda dırk seepet guncelıyor 
        //    TempData["discountStatus"] = discountStatus;//budurumu temdata saklıyoruzkı hafızada kalsın kı baskyerd e ıptal etmek ıcın kullanabılılelm
        //    return RedirectToAction(nameof(Index));
        //}



        public async Task<IActionResult> CanselApplyiedDiscount(DiscountApplyInputCode discountApplyInputCode)//165 sepete eklenen ındırımı kaldırmak 
        {
            await _basketService.CanselApplyDiscount();//iptal edıyoruz ve seppette son halınıguncelıyor
            return RedirectToAction(nameof(Index));
        }
    }
}
//DiscountApplyInputCode