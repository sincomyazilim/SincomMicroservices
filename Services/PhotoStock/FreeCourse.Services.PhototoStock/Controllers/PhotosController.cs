using FreeCourse.Services.PhototoStock.Dtos;
using FreeCourse.Shared.ControlerBase;
using FreeCourse.Shared.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FreeCourse.Services.PhototoStock.Controllers//48
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotosController : CustomBaseController// shredtakı CustomBaseController sınıfımızdan ımpelemt edıyoruz orda CreateActionResultInstance kullanmak ıcın
    {

        [HttpPost]
        public async Task<IActionResult> PhotoSave(IFormFile photoFile, CancellationToken cancellationToken)
        {//CancellationToken bu komut dosya yuklerken gecıkme olup kullanıcı sayfayı kapatırsa dosya yukleme ıslemı nerde olursa olsun dosya yuklem bıtecek

            if (photoFile != null && photoFile.Length > 0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/photos", photoFile.FileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await photoFile.CopyToAsync(stream, cancellationToken);
                }

                var returnPath = photoFile.FileName;

                PhotoDto photoDto = new PhotoDto
                {
                    Url = returnPath
                };
                return CreateActionResultInstance(ResponseDto<PhotoDto>.Success(photoDto, 200));

            }
            return CreateActionResultInstance(ResponseDto<PhotoDto>.Fail("Fotograf Seçilmedi", 400));

        }

        [HttpDelete]
        public IActionResult PhoteDelete(string photoUrl)//49 resım  sılme 
        {
            var path=Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/photos",photoUrl);
            if (!System.IO.File.Exists(path))
            {
                return CreateActionResultInstance(ResponseDto<NoContent>.Fail("fotograf bulunamadı", 404));
            }
            System.IO.File.Delete(path);

            return CreateActionResultInstance(ResponseDto<NoContent>.Success(204));
        }

    }
}
