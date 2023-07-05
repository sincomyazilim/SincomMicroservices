using FreeCourse.Web.Models.Discount;
using System.Threading.Tasks;

namespace FreeCourse.Web.Services.Abstract//162
{
    public interface IDiscountService
    {
        Task<DiscountViewModel> GetDiscount(string discountCode);
        //Task<DiscountViewModel> GetDiscount(DiscountApplyInputCode discountApplyInputCode);
       
    }
}
