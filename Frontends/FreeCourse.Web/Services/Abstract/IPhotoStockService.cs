using FreeCourse.Web.Models.PhotoStock;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace FreeCourse.Web.Services.Abstract//143
{
    public interface IPhotoStockService
    {
        Task<PhotoStockViewModel> UploadPhoto(IFormFile photo);
        Task<Boolean> DeletePhoto(string photoUrl);
    }
}
