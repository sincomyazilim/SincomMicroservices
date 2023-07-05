using FreeCourse.Web.Models.Basket;
using FreeCourse.Web.Models.Discount;
using System.Threading.Tasks;

namespace FreeCourse.Web.Services.Abstract//153
{
    public interface IBasketService22
    {
        Task<bool> SaveOrUpdateBasket(BasketViewModel22 basketViewModel);
        Task<BasketViewModel22> GetBasket();
        Task<bool> DeleteBasket();
        Task AddBasketItem(BasketItemViewModel22 basketItemViewModel);
        Task<bool> RemoveBasketItem(string courseId);
       //Task<bool> ApplyDiscount(string discountCode);
       Task<bool> ApplyDiscount(DiscountApplyInputCode input);
       
       
        Task<bool> CanselApplyDiscount();
        Task<bool> RemoveBasketItemDegerleriIptal(string courseId);
    }
}
