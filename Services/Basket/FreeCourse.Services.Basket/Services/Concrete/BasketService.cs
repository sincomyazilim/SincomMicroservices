using FreeCourse.Services.Basket.Dtos;
using FreeCourse.Services.Basket.Services.Abstract;
using FreeCourse.Shared.Dtos;
using FreeCourse.Shared.Messages.PublisherEvent;
using MassTransit;
using System.Text.Json;
using System.Threading.Tasks;

namespace FreeCourse.Services.Basket.Services.Concrete//58
{
    public class BasketService : IBasketService
    {

        private readonly RedisService _redisService;

       



        public BasketService(RedisService redisService)
        {
            _redisService = redisService;
          
        }
        //---------------------------------------------
        public async Task<ResponseDto<bool>> DeleteBasket(string userId)
        {
            var status = await _redisService.GetDb().KeyDeleteAsync(userId);
            return status ? ResponseDto<bool>.Success(204) : ResponseDto<bool>.Fail("sepet bulunamadı", 404);
        }

        public async Task<ResponseDto<BasketDto>> GetBasket(string UserId)
        {
            var existBasket = await _redisService.GetDb().StringGetAsync(UserId);//bu kullanıcının sepetı varmı
            if (string.IsNullOrEmpty(existBasket))
            {
                return ResponseDto<BasketDto>.Fail("Sepet bulunamadı", 400);
            }
            var jsonData = JsonSerializer.Deserialize<BasketDto>(existBasket);
            return ResponseDto<BasketDto>.Success(jsonData, 200);
        }

        public async Task<ResponseDto<bool>> SaveOrUpdate(BasketDto basketDto)
        {
            var status = await _redisService.GetDb().StringSetAsync(basketDto.UserId, JsonSerializer.Serialize(basketDto));         


            return status ? ResponseDto<bool>.Success(204) : ResponseDto<bool>.Fail("sepet kayıt edilmedi veya güncellenmedi", 500);
        }
    }
}
