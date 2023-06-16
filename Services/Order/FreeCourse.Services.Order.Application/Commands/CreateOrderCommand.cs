using FreeCourse.Services.Order.Application.Dtos;
using FreeCourse.Shared.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeCourse.Services.Order.Application.Commands//92
{
    public class CreateOrderCommand:IRequest<ResponseDto<CreatedOrderDto>>
    {
        public string BuyerId { get; set; }//kursu veya kursları alan kısı ıd
        public List<OrderItemDto> OrderItems { get; set; } //
        public AddressDto AddressDto { get; set; }
    }
}
