using FreeCourse.Services.FakePayment.Dtos;
using FreeCourse.Shared.ControlerBase;
using FreeCourse.Shared.Dtos;
using FreeCourse.Shared.Messages;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Reflection.Emit;
using System.Threading.Tasks;

namespace FreeCourse.Services.FakePayment.Controllers//100
{
    [Route("api/[controller]")]
    [ApiController]
    public class FakePaymentController : CustomBaseController
    {
        private readonly ISendEndpointProvider _sendEndpointProvider;//185 bunu aseckron ıcın kullanacaz

        public FakePaymentController(ISendEndpointProvider sendEndpointProvider)
        {
            _sendEndpointProvider = sendEndpointProvider;
        }
        //-------------------------------------------------------------------------------------


        //[HttpPost]
        //public IActionResult ReceivePayment(FakePaymentDto fakePaymentDto) // secron için kullanmıştık
        //{
        //    //pamentDto ile ödeme işlemi gercekleştir
        //    return CreateActionResultInstance<NoContent>(ResponseDto<NoContent>.Success(200));
        //}

        //bu metotla ıstek gelecek odeme alındı dıye, gerıye cvp gönderecel

        //---------------------------------------------------------------------------------------------------------------asenkoron
        [HttpPost]
        //[Route("/api/[controller]/ReceiveAsenkronPayment")]
        public async Task<IActionResult> ReceiveAsenkronPayment(FakePaymentAsekronDto fakePaymentAsekronDto)//asencron ıcın 
        {
            //var sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:create-order-service"));//queue
            var sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:create-order-service"));// bunu 187'de create-order-service" olarak web order apı startup ekledık



            var createOrderMessageCommand = new CreateOrderMessageCommand();//shared katmanındakı message  CreateOrderMessageCommand ornek alıyoruz 
            createOrderMessageCommand.BuyerId= fakePaymentAsekronDto.Order.BuyerId;
            createOrderMessageCommand.City= fakePaymentAsekronDto.Order.AddressDto.City;
            createOrderMessageCommand.District= fakePaymentAsekronDto.Order.AddressDto?.District;
            createOrderMessageCommand.Street= fakePaymentAsekronDto.Order.AddressDto.Street;
            createOrderMessageCommand.Line = fakePaymentAsekronDto.Order.AddressDto.Line;
            createOrderMessageCommand.ZipCode = fakePaymentAsekronDto.Order.AddressDto.ZipCode;

            fakePaymentAsekronDto.Order.OrderItems.ForEach(x =>
            {
                createOrderMessageCommand.OrderItems.Add(new OrderItem
                {
                    PictureUrl=x.PictureUrl,
                    Price=x.Price,
                    ProductId=x.ProductId,
                    ProductName=x.ProductName,

                });

            });

            await sendEndpoint.Send<CreateOrderMessageCommand>(createOrderMessageCommand);

            return CreateActionResultInstance<NoContent>(ResponseDto<NoContent>.Success(200));
        }


    }
}
// ıkıncımetoto odeme gerceklestıgınd kendı bunyeınde olan order bılgısını ılggılı rabıtmq Endpoınte send edıyor