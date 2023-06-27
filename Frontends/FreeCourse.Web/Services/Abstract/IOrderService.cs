using FreeCourse.Web.Models.Order;
using FreeCourse.Web.Models.Order.asecron;
using FreeCourse.Web.Models.Order.Secron;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FreeCourse.Web.Services.Abstract//174
{
    public interface IOrderService
    {
        /// <summary>
        /// Senkron iletişim- direk order mikroservisine istek yapılacak
        /// </summary>
        /// <param name="checkoutInfoInput"></param>
        /// <returns></returns>
        Task<OrderCreatedViewModel> CreateOrder(CheckoutInfoInput checkoutInfoInput);



        /// <summary>
        /// Asenkron iletişim- sipariş bilgileri rabbitMQ'ya gönderilecek
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        Task<OrderSuspendViewModel> SuspendOrder(CheckoutInfoInput checkoutInfoInput);



        //tum sıparıslerı alan meemed
        Task<List<OrderViewModel>> GetOrder();
    }
}
