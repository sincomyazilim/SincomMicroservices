using FreeCourse.Shared.Dtos;
using FreeCourse.Web.Models.DiscountCourse;
using FreeCourse.Web.Services.Abstract;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace FreeCourse.Web.Services.Concrete
{
    public class DiscountCourseService : IDiscountCourseService
    {
        private readonly HttpClient _httpClient;

        public DiscountCourseService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        //-------------------------------------------------------------------------------------------------------------
        public async Task<DiscountCourseViewModel> GetDiscountCourse(DiscounCoursetApplyInputCodeAndCourseId model)
        {

            var response = await _httpClient.GetAsync($"discountcourses/GetByCodeForCourse/{model}");//kode gelıyor ıstek yapırouz varmı yokmu
            if (!response.IsSuccessStatusCode)
            {
                return null;//yoksa null
            }
            var discount = await response.Content.ReadFromJsonAsync<ResponseDto<DiscountCourseViewModel>>();//varsa  viewmodel olarak döndur
            return discount.Data;
        }
    }
}

