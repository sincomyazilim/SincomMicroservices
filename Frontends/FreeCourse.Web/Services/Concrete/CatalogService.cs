using FreeCourse.Shared.Dtos;
using FreeCourse.Web.Helpers;
using FreeCourse.Web.Models.Catalog;
using FreeCourse.Web.Models.DiscountCourse;
using FreeCourse.Web.Services.Abstract;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace FreeCourse.Web.Services.Concrete//130
{
    public class CatalogService : ICatalogService
    {
        private readonly HttpClient _client;
        private readonly IPhotoStockService _photoStockService;//145
        private readonly PhotoHelper _photoHelper;
        public CatalogService(HttpClient client, IPhotoStockService photoStockService , PhotoHelper photoHelper)
        {
            _client = client;
            _photoStockService = photoStockService;
            _photoHelper = photoHelper;
        }
        //---------------------------------------------------------------------------------
        public async Task<bool> CreateCourseAsync(CreateCourseInput createCourseInput)//133
        {
            var resultPhotoService = await _photoStockService.UploadPhoto(createCourseInput.PhotoFormFile);
            if (resultPhotoService!=null)
            {
                createCourseInput.Picture = resultPhotoService.Url;
            }

            var reponse = await _client.PostAsJsonAsync<CreateCourseInput>("courses", createCourseInput);
            return reponse.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteCourseAsync(string courseId)//133 sılme ıslemı uygulanıyor gonderılen course ıd ye göre
        {
            var response =await GetByCourseId(courseId);//ders sılerken klasorekı resmıde sıılıyoruz

            await _photoStockService.DeletePhoto(response.Picture);//once sılıyoruz
               
            
            var reponse = await _client.DeleteAsync($"courses/{courseId}");
            return reponse.IsSuccessStatusCode;
        }

        public async Task<List<GetCategoryViewModel>> GetAllCategoryAsync()
        {
            var response = await _client.GetAsync("Categories");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            var responseSuccess = await response.Content.ReadFromJsonAsync<ResponseDto<List<GetCategoryViewModel>>>();
            return responseSuccess.Data;
        }

        public async Task<List<GetCourseViewModel>> GetAllCourseAsync()//133
        {
            var response = await _client.GetAsync("courses");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
           var responseSuccess=await  response.Content.ReadFromJsonAsync<ResponseDto<List<GetCourseViewModel>>>();
            responseSuccess.Data.ForEach(x =>//146
            {
                x.StockPictureUrl = _photoHelper.GetPhotoStockUrl(x.Picture);
            });
            return responseSuccess.Data;
        }

        public async Task<List<GetCourseViewModel>> GetAllCourseByUserIdAsync(string userId)//133 kullanıncn sahıb oldugu kuslar
        {
            var response = await _client.GetAsync($"courses/GetAllByUserId/{userId}");//$ işaretı varsa lınkte degısken var userId gıbı
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            var responseSuccess = await response.Content.ReadFromJsonAsync<ResponseDto<List<GetCourseViewModel>>>();

            

            responseSuccess.Data.ForEach(x =>//146
            {
                x.StockPictureUrl=_photoHelper.GetPhotoStockUrl(x.Picture);//149
            });

            return responseSuccess.Data;
        }

        public async Task<GetCourseViewModel> GetByCourseId(string courseId)//133
        {
            var response = await _client.GetAsync($"courses/{courseId}");//dolar işretı $ parametre olarak degısk oldugu ıcin eklenıyor
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            var responseSuccess = await response.Content.ReadFromJsonAsync<ResponseDto<GetCourseViewModel>>();

            responseSuccess.Data.StockPictureUrl = _photoHelper.GetPhotoStockUrl(responseSuccess.Data.Picture);

            return responseSuccess.Data;
        }

        public async Task<bool> UpdateCourseAsync(UpdateCourseInput updateCourseInput)//update işlemı
        {
            var resultPhotoService = await _photoStockService.UploadPhoto(updateCourseInput.PhotoFormFile);//148 eklendi
            if (resultPhotoService != null)
            {
                await _photoStockService.DeletePhoto(updateCourseInput.Picture);//once sılıyoruz
                updateCourseInput.Picture = resultPhotoService.Url;//yenısınıeklıyoruz
            }
            updateCourseInput.StockPictureUrl = _photoHelper.GetPhotoStockUrl(updateCourseInput.Picture);//eski resım gelsın dıye
            var reponse = await _client.PutAsJsonAsync<UpdateCourseInput>("courses", updateCourseInput);
            return reponse.IsSuccessStatusCode;
        }


       

       
    }
}
