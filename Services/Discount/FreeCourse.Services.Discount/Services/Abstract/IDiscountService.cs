using FreeCourse.Shared.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FreeCourse.Services.Discount.Services.Abstract
{
    public interface IDiscountService//70
    {
        Task<ResponseDto<List<Models.Discount>>>GetAll();
        Task<ResponseDto<Models.Discount>>GetById(int id);
        Task<ResponseDto<NoContent>>Save(Models.Discount discount);
        Task<ResponseDto<NoContent>>Update(Models.Discount discount);
        Task<ResponseDto<NoContent>>Delete(int id);

        Task<ResponseDto<Models.Discount>>GetByCodeAndUserId(string code, string userId);//bu kullanıcının  bu code ıle ındımı olmusmu 
       // Task<ResponseDto<Models.Discount>>GetByCodeAndUserIdAndCourseId(string code, string userId,string courseId);//bu kullanıcının  bu code ıle ındımı olmusmu 
    }
}
