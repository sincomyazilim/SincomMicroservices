using FreeCourse.Services.Order.Application.Commands;
using FreeCourse.Services.Order.Application.Queries;
using FreeCourse.Shared.ControlerBase;
using FreeCourse.Shared.Services.Abstract;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FreeCourse.Services.Order.API.Controllers//95
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : CustomBaseController
    {
        private readonly IMediator _mediator;
        private readonly ISharedIdentityService _sharedIdentityService;

        public OrdersController(IMediator mediator, ISharedIdentityService sharedIdentityService)
        {
            _mediator = mediator;
            _sharedIdentityService = sharedIdentityService;
        }
        //--------------------------------------------------------------------------------

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var userId = _sharedIdentityService.GetUserId;
            var response = await _mediator.Send(new GetOrderByUserIdQuery { UserId = userId });
            return CreateActionResultInstance(response);
        }

        [HttpPost]
        public async Task<IActionResult>SaveOrder(CreateOrderCommand createOrderCommand)
        {
            
            var response=await _mediator.Send(createOrderCommand);
            return CreateActionResultInstance(response);
        }
    }
}
