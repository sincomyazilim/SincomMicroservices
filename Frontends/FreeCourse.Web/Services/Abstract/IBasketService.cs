using FreeCourse.Web.Models.Basket;
using FreeCourse.Web.Models.Discount;
using System.Threading.Tasks;

namespace FreeCourse.Web.Services.Abstract//153
{
    public interface IBasketService
    {
        Task<bool> SaveOrUpdateBasket(BasketViewModel basketViewModel);
        Task<BasketViewModel> GetBasket();
        Task<bool> DeleteBasket();
        Task AddBasketItem(BasketItemViewModel basketItemViewModel);
        Task<bool> RemoveBasketItem(string courseId);
       Task<bool> ApplyDiscount(string discountCode);
       
        //Task<bool> ApplyDiscount(DiscountApplyInputCode discountApplyInputCode);
        Task<bool> CanselApplyDiscount();
    }
}
