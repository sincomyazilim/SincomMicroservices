using FreeCourse.Shared.Dtos;
using FreeCourse.Web.Models.Basket;
using FreeCourse.Web.Models.Discount;
using FreeCourse.Web.Services.Abstract;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks; 

namespace FreeCourse.Web.Services.Concrete//154
{
    public class BasketService22 : IBasketService22
    {
        private readonly HttpClient _httpClient;
        private readonly IDiscountService _discountService;


        public BasketService22(HttpClient httpClient, IDiscountService discountService)
        {
            _httpClient = httpClient;
            _discountService = discountService;
        }
        //-----------------------------------------------------------------------
        public async Task AddBasketItem(BasketItemViewModel22 basketItemViewModel)//155
        {
            var basket = await GetBasket();//courseId lı basketı getır 
            if (basket != null)//geldı ıse
            {
                if (!basket.BasketItems.Any(x => x.CourseId == basketItemViewModel.CourseId))//aynı oılanı ekleme
                {
                    basket.BasketItems.Add(basketItemViewModel);//aynı degılse ekle
                }
            }
            else//eger basket bos geldıyse yanıyoktur
            {
                basket = new BasketViewModel22();
                
                basket.BasketItems.Add(basketItemViewModel);
              
                //basket = new BasketViewModel
                //{//BU KOD KULLANMIYORUM CUNKU BasketViewModel NEWLENDIGINDE CTOR METODUNA  _basketItems = new List<BasketItemViewModel>(); EKLIYORUZ
                //    BasketItems = new System.Collections.Generic.List<BasketItemViewModel> 
                //    { 
                //        basketItemViewModel 
                //    }
                    
                //};


            }
            await SaveOrUpdateBasket(basket);//kayıt et
        }
        //-----------------------------------------------------------------------------------------------------------------------
        public async Task<bool> ApplyDiscount(DiscountApplyInputCode input)//164 doldurudk dıscount olusturdukondan sonra
        {
            await CanselApplyDiscount();//daha önce ındıırm olmussa ıptalet
            var basket = await GetBasket();
            if (basket == null)
            {
                return false;
            }
            var hasdiscount = await _discountService.GetDiscount(input.Code);
            if (hasdiscount == null)
            {
                return false;
            }
            if (hasdiscount.CourseId != input.CourseId)
            {
                return false;
            }
            else
            {
                basket.BasketItems.ForEach(x =>
                {
                    if (x.CourseId == hasdiscount.CourseId)
                    {
                        basket.ApplyDiscount(hasdiscount.CourseId, hasdiscount.Code, hasdiscount.Rate, hasdiscount.Status);
                        basket.BasketItems.ForEach(b => { b.ApplyDiscount(hasdiscount.CourseId, hasdiscount.Code, hasdiscount.Rate); });
                    }


                });

                //basket.ApplyDiscount(hasdiscount.Code, hasdiscount.Rate, hasdiscount.CourseId);  //sepetı guncellıyoruz bunada metot yazıyoruz 167

                await SaveOrUpdateBasket(basket);//son halınıkayıt veye update edıyoruz
                return true;
            }
         
        }



        public async Task<bool> CanselApplyDiscount()//164 doldurudk dıscount olusturdukondan sonra
        {
            var basket=await GetBasket();
            if (basket==null||basket.DiscountCode==null)//basket bos gelırse false dön
            {
                return false;
            }
            basket.CanselDiscount();//basket ıptal edılmıs son halını guncelle,,null yaptık buraya metoto yazdık 167
        
            await SaveOrUpdateBasket(basket);//guncellenen sepetı savechange et
            return true;
        }
      
        public async Task<bool> DeleteBasket()//155
        {
            var result = await _httpClient.DeleteAsync("baskets");

            return result.IsSuccessStatusCode;
        }

        public async Task<BasketViewModel22> GetBasket()//155
        {
            var response = await _httpClient.GetAsync("baskets");//burası lınke ıstek yapıypr
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            var basketViewModel = await response.Content.ReadFromJsonAsync<ResponseDto<BasketViewModel22>>();
            return basketViewModel.Data;
        }

        public async Task<bool> RemoveBasketItem(string courseId)//155
        {
           var basket=await GetBasket();
            if (basket==null)
            {
                return false;
            }
            var deleteBasketItem=basket.BasketItems.FirstOrDefault(x=>x.CourseId==courseId);//seppettıkı ıtemı secıyor
            if (deleteBasketItem==null)
            {
                return false;
            }

            var deleteResult=basket.BasketItems.Remove(deleteBasketItem);//basketıtemlerı tek tek sılıyoruz ıstege baglı--- burda ectıgı ıtemı ıslıyor
            if (!deleteResult)
            {
                return false;//sılınmedıyse  false don
            }
            if (!basket.BasketItems.Any())//basket ıcındekı basketıtemlar bos ıse---secılecek bırsey kalamdıysa  ındırım kodunu bosalt
            {
                basket.CourseIdForBasket = null;
                basket.DiscountCode = null;//codu boşa al
                basket.DiscountRate = null;//codu boşa al
                basket.Status = true;
              
            }



           return await SaveOrUpdateBasket(basket);//son kısımını kayıt ettım veya gunceleldım true donuyorum

        }




        public async Task<bool> RemoveBasketItemDegerleriIptal(string courseId)//155
        {
            var basket = await GetBasket();
            if (basket == null)
            {
                return false;
            }
            var deleteBasketItem = basket.BasketItems.FirstOrDefault(x => x.CourseId == courseId);//seppettıkı ıtemı secıyor
            if (deleteBasketItem == null)
            {
                return false;
            }
           
            var deleteBasketItemDegerleri = basket.BasketItems.FirstOrDefault(x => x.CourseId == courseId).Degerler.FirstOrDefault(x=>x.CourseId==courseId);//seppettıkı ıtemı secıyor
            if (deleteBasketItemDegerleri == null)
            {
                return false;
            }
            
            

            var deleteResult = basket.BasketItems.FirstOrDefault(x => x.CourseId == courseId).Degerler.Remove(deleteBasketItemDegerleri);//basketıtemlerı tek tek sılıyoruz ıstege baglı--- burda ectıgı ıtemı ıslıyor
            if (!deleteResult)
            {
                return false;//sılınmedıyse  false don
            }
            foreach (var item in basket.BasketItems)
            {
                if (item.CourseId==courseId)
                {
                    item.Status = false;
                }
            }
            return await SaveOrUpdateBasket(basket);//son kısımını kayıt ettım veya gunceleldım true donuyorum

        }



        public async Task<bool> SaveOrUpdateBasket(BasketViewModel22 basketViewModel)//155
        {
            var response = await _httpClient.PostAsJsonAsync<BasketViewModel22>("baskets",basketViewModel);
            return response.IsSuccessStatusCode;
        }
    }
}
