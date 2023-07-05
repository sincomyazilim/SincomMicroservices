using FreeCourse.Services.Discount.Dtos;
using FreeCourse.Shared.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FreeCourse.Services.Discount.Services.Abstract
{
    public interface IDiscountService//70
    {
        Task<ResponseDto<List<Models.DiscountEski>>>GetAll();
        Task<ResponseDto<Models.DiscountEski>>GetById(int id);
        Task<ResponseDto<NoContent>>Save(Models.DiscountEski discount);
        Task<ResponseDto<NoContent>>Update(Models.DiscountEski discount);
        Task<ResponseDto<NoContent>>Delete(int id);

        Task<ResponseDto<Models.DiscountEski>>GetByCodeAndUserId(string code, string userId);//bu kullanıcının  bu code ıle ındımı olmusmu 
       // Task<ResponseDto<Models.DiscountEski>> GetByCodeAndUserIdandCourseId(string userId, GetCodeandCorseIdDto getCodeandCorseIdDto);


    }
}
