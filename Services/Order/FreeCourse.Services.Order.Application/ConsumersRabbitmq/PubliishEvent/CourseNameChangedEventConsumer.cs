using FreeCourse.Services.Order.Infrastructure.Context;
using FreeCourse.Shared.Messages.PublisherEvent;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeCourse.Services.Order.Application.ConsumersRabbitmq.PubliishEvent//191
{
    public class CourseNameChangedEventConsumer : IConsumer<CourseNameChangedEvent>
    {
        private readonly OrderDbContext _orderDbContext;

        public CourseNameChangedEventConsumer(OrderDbContext orderDbContext)
        {
            _orderDbContext = orderDbContext;
        }
        //------------------------------------------------------------------------
        public async Task Consume(ConsumeContext<CourseNameChangedEvent> context)
        {
            var orderItems = await _orderDbContext.OrderItems.Where(x => x.ProductId == context.Message.CourseId).ToListAsync();

            foreach (var item in orderItems)
            {
                item.UpdateOrderItem(context.Message.UpdateName, item.PictureUrl, item.Price);
            }
            await _orderDbContext.SaveChangesAsync();
        }
    }
}


//catalog dakı courserservıce ıcındekı update kısmı bır evet fırtaltıyor burasıda orayı yakalyor kendı prductname degıstıryor

//startup ta bu kısmı tanımlıyoruz