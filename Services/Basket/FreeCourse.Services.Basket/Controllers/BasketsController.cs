using FreeCourse.Services.Basket.Dtos;
using FreeCourse.Services.Basket.Services.Abstract;
using FreeCourse.Services.Basket.Services.Concrete;
using FreeCourse.Shared.ControlerBase;
using FreeCourse.Shared.Services.Abstract;
using FreeCourse.Shared.Services.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace FreeCourse.Services.Basket.Controllers//60
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketsController : CustomBaseController//CustomBaseController bundan mpelmat etmesının seebı responsedto kullnmak ıcın
    {
        private readonly IBasketService _ibasketService;
        private readonly ISharedIdentityService _isharedIdentityService;

        public BasketsController(IBasketService basketService, ISharedIdentityService sharedIdentityService)
        {
            _ibasketService = basketService;
            _isharedIdentityService = sharedIdentityService;
        }
        //-----------------------------------------------------------------------

        [HttpGet]
        public async Task<IActionResult> GetBasket()
        {
            //var claims = User.Claims;
            return CreateActionResultInstance(await _ibasketService.GetBasket(_isharedIdentityService.GetUserId));
            //var userId = _isharedIdentityService.GetUserId;
            //var getBasket = await _ibasketService.GetBasket(userId);
            //return CreateActionResultInstance(getBasket);
        }

        [HttpPost]
        public async Task<IActionResult>SaveOrdUpdateBasket(BasketDto basketDto)
        {
            basketDto.UserId =_isharedIdentityService.GetUserId;//154 uuserId burdan gönderıyoırum tekrar consume tmeye gerek yok
            var response=await _ibasketService.SaveOrUpdate(basketDto);
            return CreateActionResultInstance(response);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteBasket()
        {
            var userId=_isharedIdentityService.GetUserId;
            var deleteBasket=await _ibasketService.DeleteBasket(userId);
            return CreateActionResultInstance(deleteBasket);
        }
    }
}
