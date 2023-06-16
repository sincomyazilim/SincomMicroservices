using FreeCourse.Services.Order.Application.Dtos;
using FreeCourse.Shared.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeCourse.Services.Order.Application.Queries//91
{
    public class GetOrderByUserIdQuery:IRequest<ResponseDto<List<OrderDto>>>
    {
        public string UserId { get; set; }
    }
}
