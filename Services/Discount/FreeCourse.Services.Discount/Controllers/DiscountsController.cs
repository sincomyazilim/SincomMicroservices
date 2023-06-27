﻿using FreeCourse.Services.Discount.Services.Abstract;
using FreeCourse.Shared.ControlerBase;
using FreeCourse.Shared.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FreeCourse.Services.Discount.Controllers//75
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountsController : CustomBaseController
    {
        private readonly IDiscountService _discountService;
        private readonly ISharedIdentityService _sharedIdentityService;
        

        public DiscountsController(IDiscountService discountService, ISharedIdentityService sharedIdentityService)
        {
            _discountService = discountService;
            _sharedIdentityService = sharedIdentityService;
          
        }
        //-------------------------------------------------------------------------

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return CreateActionResultInstance(await _discountService.GetAll());
        }
        // localhost/api/discount/5
        [HttpGet("{id}")]
        public async Task<IActionResult>GetById(int id)
        {
            var discount=await _discountService.GetById(id);
            return CreateActionResultInstance(discount);
        }
        [HttpGet]
        [Route("/api/[controller]/[action]/{code}")]
        
        public async Task<IActionResult>GetByCode(string code)
        {
            var userId = _sharedIdentityService.GetUserId;
            var discount=await _discountService.GetByCodeAndUserId(code,userId);
            return CreateActionResultInstance(discount);
        }



        //[HttpGet]
        //[Route("/api/[controller]/[action]/{code}")]

        //public async Task<IActionResult> GetByCodeForCourse(string code, string courseId)
        //{
        //    //var courseIdFordiscount = await _courseService.GetByIdAsync(courseId);
        //    var userId = _sharedIdentityService.GetUserId;
        //    var discount = await _discountService.GetByCodeAndUserIdAndCourseId(code, userId,courseId);
        //    return CreateActionResultInstance(discount);
        //}






        [HttpPost]
        public async Task<IActionResult> Save(Models.Discount discount)
        {
            return CreateActionResultInstance(await _discountService.Save(discount));
        }
        
        [HttpPut]
        public async Task<IActionResult>Update(Models.Discount discount)
        {
            return CreateActionResultInstance(await _discountService.Update(discount));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return CreateActionResultInstance(await _discountService.Delete(id));
        }
    }
}
