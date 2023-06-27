using FreeCourse.Web.Models.Basket;
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
        //Task<bool> ApplyDiscountForCourse(string discountCode, string courseId);
        Task<bool> CanselApplyDiscount();
    }
}
