using FreeCourse.Services.Order.Infrastructure.Context;
using FreeCourse.Shared.Messages;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace FreeCourse.Services.Order.Application.ConsumersRabbitmq//187 kuruktakı verılerı consume edecek
{
    public class CreateOrderMessageCommandConsumer : IConsumer<CreateOrderMessageCommand>//IConsumer MassTransit gelıyor
    {

        private readonly OrderDbContext _orderDbContext;

        public CreateOrderMessageCommandConsumer(OrderDbContext orderDbContext)
        {
            _orderDbContext = orderDbContext;
        }
        //-------------------------------------------------------------------------------
        public async Task Consume(ConsumeContext<CreateOrderMessageCommand> context)
        {
            var newAdress = new Domain.OrderAggregate.Addres(context.Message.City, context.Message.District, context.Message.Street, context.Message.ZipCode, context.Message.Line);  //kurultakı adresı cekıyoruz

            Domain.OrderAggregate.Order order = new Domain.OrderAggregate.Order(context.Message.BuyerId, newAdress);//order sınıf uretıp adress ver buyerId ekledık

            context.Message.OrderItems.ForEach(b =>//olusan order sınıfını orderItemlarını eklıyoruz
            {
                order.AddOrderItem(b.ProductId, b.ProductName, b.Price, b.PictureUrl);
            });

            await _orderDbContext.Orders.AddAsync(order);//order verıtaban eklıyoruz
            await _orderDbContext.SaveChangesAsync();
        }
    }
}


// bu ıslemden haberder etmek ıcın apı katmanın startup  ekleyecez 187