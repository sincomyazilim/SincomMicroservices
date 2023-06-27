using FreeCourse.Web.Models.Order;
using FreeCourse.Web.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FreeCourse.Web.Controllers//176
{
    public class OrderController : Controller
    {
        private readonly IBasketService _basketService;
        private readonly IOrderService _orderService;

        public OrderController(IBasketService basketService, IOrderService orderService)
        {
            _basketService = basketService;
            _orderService = orderService;
        }
        //----------------------------------------------------------------------
       
        public async Task<IActionResult> Checkout()
        {
            var basket=await _basketService.GetBasket();
            ViewBag.basket=basket;
            return View(new CheckoutInfoInput());
        }
        [HttpPost]
        public async Task<IActionResult> Checkout(CheckoutInfoInput input)
        {
            var orderStatus=await _orderService.CreateOrder(input);
            if (!orderStatus.IsSuccessful)
            {
                //TempData["error"] = orderStatus.Error;//bir istekten dıger istege data tasımak ıstıyorsak TempData kullanılır yada bunu ıptal edıl 2 you yazacaz
               // return RedirectToAction(nameof(Checkout));


                var basket = await _basketService.GetBasket();
                ViewBag.basket = basket;
                ViewBag.error = orderStatus.Error;
                return View();
                
            }
            

            return RedirectToAction(nameof(SuccessfulCheckout), new {orderId=orderStatus.OrderId});

        }

        public IActionResult SuccessfulCheckout(int orderId)
        {
            ViewBag.orderId=orderId;
            return View();
        }
    }
}
