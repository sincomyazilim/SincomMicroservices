using FreeCourse.Services.Basket.Dtos;
using FreeCourse.Shared.Dtos;
using System.Threading.Tasks;

namespace FreeCourse.Services.Basket.Services.Abstract
{
    public interface IBasketService//56 
    {
        Task<ResponseDto<BasketDto>>GetBasket(string UserId);
        Task<ResponseDto<bool>>SaveOrUpdate(BasketDto basketDto);
        Task<ResponseDto<bool>>DeleteBasket(string userId);
    }
}
