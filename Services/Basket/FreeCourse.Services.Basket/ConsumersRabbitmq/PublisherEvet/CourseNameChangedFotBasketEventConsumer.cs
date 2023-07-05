using FreeCourse.Services.Basket.Dtos;
using FreeCourse.Services.Basket.Services.Abstract;
using FreeCourse.Services.Basket.Services.Concrete;
using FreeCourse.Shared.Messages.PublisherEvent;
using FreeCourse.Shared.Services.Abstract;
using MassTransit;
using System.Text.Json;
using System.Threading.Tasks;

namespace FreeCourse.Services.Basket.ConsumersRabbitmq.PublisherEvet
{
    public class CourseNameChangedFotBasketEventConsumer : IConsumer<CourseNameChangedEvent>
    {
        private readonly RedisService _redisService;

        public CourseNameChangedFotBasketEventConsumer(RedisService redisService)
        {
            _redisService = redisService;
        }



        //-----------------------------------------------------------------------------
        public async Task Consume(ConsumeContext<CourseNameChangedEvent> context)
        {
            var keys=_redisService.GetKeys();
            if (keys!=null)
            {
                foreach (var key in keys)
                {
                    var basket=await _redisService.GetDb().StringGetAsync(key);
                    var basketDto=JsonSerializer.Deserialize<BasketDto>(basket);
                    basketDto.BasketItems.ForEach(x =>
                    {
                        if (x.CourseId == context.Message.CourseId)
                        {
                            x.CourseName = context.Message.UpdateName;
                        }
                        else
                        {
                            x.CourseName = x.CourseName;
                        }
                    });
                    await _redisService.GetDb().StringSetAsync(key,JsonSerializer.Serialize(basketDto));
                }
            }
           
        }
    }
}
