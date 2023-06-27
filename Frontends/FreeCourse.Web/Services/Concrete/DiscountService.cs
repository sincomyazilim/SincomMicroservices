

using FreeCourse.Shared.Dtos;
using FreeCourse.Web.Models.Discount;
using FreeCourse.Web.Services.Abstract;
using System;
using System.Net.Http;
using System.Net.Http.Json;
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
            var response=await _httpClient.GetAsync($"discounts/GetByCode/{discountCode}");//kode gelıyor ıstek yapırouz varmı yokmu
            if (!response.IsSuccessStatusCode)
            {
                return null;//yoksa null
            }
            var discount = await response.Content.ReadFromJsonAsync<ResponseDto<DiscountViewModel>>();//varsa  viewmodel olarak döndur
            return discount.Data;
        }

        //public async Task<DiscountForCourseViewModel> GetDiscountForCourse(DiscountApplyInputCodeAndCourseId model)//163
        //{                      
        //    var response = await _httpClient.GetAsync($"discounts/GetByCodeForCourse/{model}");//kode gelıyor ıstek yapırouz varmı yokmu
        //    if (!response.IsSuccessStatusCode)
        //    {
        //        return null;//yoksa null
        //    }
        //    var discount = await response.Content.ReadFromJsonAsync<ResponseDto<DiscountForCourseViewModel>>();//varsa  viewmodel olarak döndur
        //    return discount.Data;
        //}
    }
}
