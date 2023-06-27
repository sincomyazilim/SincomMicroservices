using FreeCourse.Services.FakePayment.Dtos;
using FreeCourse.Shared.ControlerBase;
using FreeCourse.Shared.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Emit;

namespace FreeCourse.Services.FakePayment.Controllers//100
{
    [Route("api/[controller]")]
    [ApiController]
    public class FakePaymentController : CustomBaseController
    {
        [HttpPost]
        public IActionResult ReceivePayment(FakePaymentDto fakePaymentDto) 
        {
            //pamentDto ile ödeme işlemi gercekleştir
            return CreateActionResultInstance<NoContent>(ResponseDto<NoContent>.Success(200));
        }
    }
}
//bu metotla ıstek gelecek odeme alındı dıye, gerıye cvp gönderecel