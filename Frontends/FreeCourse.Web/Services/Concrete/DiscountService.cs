

using FreeCourse.Shared.Dtos;
using FreeCourse.Web.Models.Discount;
using FreeCourse.Web.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace FreeCourse.Web.Services.Concrete//162
{
    public class DiscountService : IDiscountService
    {
        private readonly HttpClient _httpClient;

        public DiscountService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        //---------------------------------------------------------------
        public async Task<DiscountViewModel> GetDiscount(string discountCode)//163
        {                                             //[controller]/[action]/{code}")
            var response = await _httpClient.GetAsync($"discounts/GetByCode/{discountCode}");//kode gelıyor ıstek yapırouz varmı yokmu
            if (!response.IsSuccessStatusCode)
            {
                return null;//yoksa null
            }
            var discount = await response.Content.ReadFromJsonAsync<ResponseDto<DiscountViewModel>>();//varsa  viewmodel olarak döndur
            return discount.Data;
        }






        //[HttpGet]
        //public async Task<DiscountViewModel> GetDiscount(DiscountApplyInputCode discountApplyInputCode)//163
        //{
        //    //var discountApplyInputCode2=new DiscountApplyInputCode();
        //    //discountApplyInputCode2.Code = discountApplyInputCode.Code;
        //    //discountApplyInputCode2.CourseId = discountApplyInputCode.CourseId;

        //    var response = await _httpClient.GetAsync($"discounts/GetByCodeandCourseIdCourseId/{discountApplyInputCode}");//kode gelıyor ıstek yapırouz varmı yokmu
        //    if (!response.IsSuccessStatusCode)
        //    {
        //        return null;//yoksa null
        //    }
        //    var discount = await response.Content.ReadFromJsonAsync<ResponseDto<DiscountViewModel>>();//varsa  viewmodel olarak döndur
        //    return discount.Data;
        //}
    }
}
