using FreeCourse.Web.Models;
using Microsoft.Extensions.Options;

namespace FreeCourse.Web.Helpers
{
    public class PhotoHelper//146
    {
        private readonly ServiceApiSettings _serviceApiSettings;

        public PhotoHelper(IOptions<ServiceApiSettings> serviceApiSettings)
        {
            _serviceApiSettings = serviceApiSettings.Value;
        }

        public string GetPhotoStockUrl(string photoUrl)//services/photoStock/photos/123.jpg gıbı
        {
            return $"{_serviceApiSettings.PhotoStockUrl}/photos/{photoUrl}";//klasorden resım yolu alıyoruz
        }



    }
}




//photo mıcroservıse baglanıp fotograf yopunu alıyoruz