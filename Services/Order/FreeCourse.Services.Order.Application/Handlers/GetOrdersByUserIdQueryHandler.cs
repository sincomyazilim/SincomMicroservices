using FreeCourse.Services.Order.Application.Dtos;
using FreeCourse.Services.Order.Application.Mapping;
using FreeCourse.Services.Order.Application.Queries;
using FreeCourse.Services.Order.Infrastructure.Context;
using FreeCourse.Shared.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FreeCourse.Services.Order.Application.Handlers//91
{
   public class GetOrdersByUserIdQueryHandler : IRequestHandler<GetOrderByUserIdQuery, ResponseDto<List<OrderDto>>>
    {
        private readonly OrderDbContext _context;

        public GetOrdersByUserIdQueryHandler(OrderDbContext context)
        {
            _context = context;
        }
        //------------------------------------------------------------------
        public async Task<ResponseDto<List<OrderDto>>> Handle(GetOrderByUserIdQuery request, CancellationToken cancellationToken)
        {

            var orders=await _context.Orders.Include(x=>x.OrderItems).Where(x=>x.BuyerId==request.UserId).ToListAsync();
            if (!orders.Any())
            {
                return ResponseDto<List<OrderDto>>.Success(new List<OrderDto>(), 200);
            }
            var ordersDto= CustomObjectMapper.Mapper.Map<List<OrderDto>>(orders);

            return ResponseDto<List<OrderDto>>.Success(ordersDto, 200);
        }
    }
}
