using FreeCourse.Shared.Dtos;
using FreeCourse.Shared.Services.Abstract;
using FreeCourse.Web.Models.FakePaymnet;
using FreeCourse.Web.Models.Order;
using FreeCourse.Web.Models.Order.asecron;
using FreeCourse.Web.Models.Order.Secron;
using FreeCourse.Web.Services.Abstract;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace FreeCourse.Web.Services.Concrete//174-175 doldruyoruz
{
    public class OrderService : IOrderService
    {

        private readonly IFakePaymentService _fakePaymentService;
        private readonly HttpClient _httpClient;
        private readonly IBasketService22 _basketService;
        private readonly ISharedIdentityService _sharedIdentityService;

        public OrderService(IFakePaymentService fakePaymentService, HttpClient httpClient, IBasketService22 basketService, ISharedIdentityService sharedIdentityService)
        {
            _fakePaymentService = fakePaymentService;
            _httpClient = httpClient;
            _basketService = basketService;
            _sharedIdentityService = sharedIdentityService;
        }
        //----------------------------------------------------------------------------------------------------

        public async Task<OrderCreatedViewModel> CreateOrder(CheckoutInfoInput checkoutInfoInput)//sekron
        {
            var basket = await _basketService.GetBasket();//totalprice almak ıcın asketı cagırdık
            var fakePaymentInfoInput = new FakePaymentInfoInput()//fakepayment ıcın gıdecek kart bılgılerını doldurudk
            {
                CardName = checkoutInfoInput.CardName,
                CardNumber = checkoutInfoInput.CardNumber,
                Expiration = checkoutInfoInput.Expiration,
                CVV = checkoutInfoInput.CVV,
                TotalPrice = basket.TotalPrice,
            };
            var responsePayment = await _fakePaymentService.ReceivePayment(fakePaymentInfoInput);//fakepamet servıce gonderdık 
            if (!responsePayment)
            {
                return new OrderCreatedViewModel()//odemem gerceklesmezse bu mesajı ver
                {
                    Error = "Ödeme alinamadı",
                    IsSuccessful = false
                };
            }
            //BURDA SECRON MANTIGI VAR ONCER ODEME ALDIK SONRA SIPARIS OLDURTUDUK
            var orderCreateInput = new OrderCreateInput()//odeme gercekelsıtı artık order dldru ve kayıt et
            {
                BuyerId = _sharedIdentityService.GetUserId,
                AddressDto = new AddressCreateInput()
                {
                    City = checkoutInfoInput.City,
                    District = checkoutInfoInput.District,
                    Street = checkoutInfoInput.Street,
                    ZipCode = checkoutInfoInput.ZipCode,
                    Line = checkoutInfoInput.Line
                },

            };
            basket.BasketItems.ForEach(x =>
            {
                var orderItem = new OrderItemCreateInput()
                {
                    ProductId = x.CourseId,
                    Price = x.GetCurrentPrice,
                    PictureUrl = "",
                    ProductName = x.CourseName


                };
                orderCreateInput.OrderItems.Add(orderItem);//order doldurduk
            });


            var response = await _httpClient.PostAsJsonAsync<OrderCreateInput>("orders", orderCreateInput);//order kaydedıyorzu
            if (!response.IsSuccessStatusCode)
            {
                return new OrderCreatedViewModel()//odemem gerceklesmezse bu mesajı ver
                {
                    Error = "Sipariş oluşturulamadı",
                    IsSuccessful = false
                };
            }

            var orderCreatedViewModel = await response.Content.ReadFromJsonAsync<ResponseDto<OrderCreatedViewModel>>();
            orderCreatedViewModel.Data.IsSuccessful = true;
            await _basketService.DeleteBasket();//sepetı temızlıyoruz
            return orderCreatedViewModel.Data;

            //bu senryoda önce odeme aldık true bılgısıgeldı order kayıt ettık false gelse order kayıt edılmez.
        }
        //--------------------------------------------------------------------------------------------
        public async Task<List<OrderViewModel>> GetOrder()
        {
            var repsonse = await _httpClient.GetFromJsonAsync<ResponseDto<List<OrderViewModel>>>("orders");//sıparıslero sırala
            return repsonse.Data;
        }
        //--- ASENCRON BOLUMUDUR  185 VİDEO DOLDURACAZ
     
        
        
        
        
        public async Task<OrderSuspendViewModel> SuspendOrder(CheckoutInfoInput checkoutInfoInput)//asencron bolumudur
        {
            var basket = await _basketService.GetBasket();//basketı getırdık


            var orderCreateInput = new OrderCreateInput()//sipariş on bılgıler doldurduk bura adress bılgılerını doldurduk
            {
                BuyerId = _sharedIdentityService.GetUserId,
                AddressDto = new AddressCreateInput()
                {
                    City = checkoutInfoInput.City,
                    District = checkoutInfoInput.District,
                    Street = checkoutInfoInput.Street,
                    ZipCode = checkoutInfoInput.ZipCode,
                    Line = checkoutInfoInput.Line
                },

            };
           

            basket.BasketItems.ForEach(x =>                      //burda urunbılgılerını  doldurduk
            {
                var orderItem = new OrderItemCreateInput()
                {
                    ProductId = x.CourseId,
                    Price = x.GetCurrentPrice,
                    PictureUrl = "",
                    ProductName = x.CourseName


                };
                orderCreateInput.OrderItems.Add(orderItem);//order doldurduk
            });

       //BURAYA KADAR KART VE SIPARIS BILGILERI VE ÖDENECEK MIKAR(BASKET TEN CEKILDI) ALINDI VE CAGIRDIGIMIZ BASKET ICIUDEN ALINCAKLARIN BILGISINI  ASAGIDA YAZIYRUZ


            var fakePaymenAsenkrontInfoInput = new FakePaymenAsenkrontInfoInput()//fakepayment ıcın gıdecek kart bılgılerını doldurudk içine order bılgılerde ekledık
            {
                CardName = checkoutInfoInput.CardName,
                CardNumber = checkoutInfoInput.CardNumber,
                Expiration = checkoutInfoInput.Expiration,
                CVV = checkoutInfoInput.CVV,
                TotalPrice = basket.TotalPrice,


                Order= orderCreateInput,// asecronda burayı ekledık fazladan  order bılgısını burda eklıyoruz odeme gelıdgınde sıparsı olussun dıye
            };

            var responsePayment = await _fakePaymentService.ReceiveAsenkronPayment(fakePaymenAsenkrontInfoInput);//fakepamet servıce gonderdık 
            if (!responsePayment)
            {
                return new OrderSuspendViewModel()//odemem gerceklesmezse bu mesajı ver
                {
                    Error = "Ödeme alinamadı",
                    IsSuccessful = false
                };
            }

            await _basketService.DeleteBasket();//sepetı temızlıyoruz

            return new OrderSuspendViewModel() { IsSuccessful = true };




        }
    }
}
