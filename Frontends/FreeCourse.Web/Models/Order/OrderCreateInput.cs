using System.Collections.Generic;

namespace FreeCourse.Web.Models.Order//175
{
    public class OrderCreateInput
    {
        public OrderCreateInput()
        {
            OrderItems = new List<OrderItemCreateInput>();//175 bunu eklıyoruz orderItemları eklemekıstersen otamatık newlesn
        }


        public string BuyerId { get; set; }//kursu veya kursları alan kısı ıd
        public List<OrderItemCreateInput> OrderItems { get; set; } //
        public AddressCreateInput AddressDto { get; set; }
    }
}
