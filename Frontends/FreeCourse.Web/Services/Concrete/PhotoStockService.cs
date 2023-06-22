using FreeCourse.Shared.Dtos;
using FreeCourse.Web.Models.PhotoStock;
using FreeCourse.Web.Services.Abstract;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace FreeCourse.Web.Services.Concrete//143
{
    public class PhotoStockService : IPhotoStockService
    {
        private readonly HttpClient _httpClient;

        public PhotoStockService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        //-------------------------------------------------------------------------
        public async Task<bool> DeletePhoto(string photoUrl)//144 içini doldurduk
        {
            var response=await _httpClient.DeleteAsync($"photos?photoUrl={photoUrl}");
            return response.IsSuccessStatusCode;
        }

        public async Task<PhotoStockViewModel> UploadPhoto(IFormFile photo)//144
        {
            if (photo==null||photo.Length<=0)
            {
                return null;
            }
            //örnek dosya ismi=21212322233.jpg
            var randomFileName = $" {Guid.NewGuid().ToString()}{Path.GetExtension(photo.FileName)}";//random sayıdan olusan ısım 

            using var ms =new  MemoryStream();//dosyayı bınarıy cevııryoruz
            await photo.CopyToAsync(ms);//

            var multipartContent = new MultipartFormDataContent();
            multipartContent.Add(new ByteArrayContent(ms.ToArray()), "photoFile", randomFileName);// phptpstock mıcroservste photoFile ismine göre olacak

            var response = await _httpClient.PostAsync("Photos", multipartContent);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            
            var responseSuccess= await response.Content.ReadFromJsonAsync<ResponseDto<PhotoStockViewModel>>();//urldöndur
            return responseSuccess.Data;
        }
    }
}
