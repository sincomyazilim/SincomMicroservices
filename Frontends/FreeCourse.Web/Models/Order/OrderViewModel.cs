using System.Collections.Generic;
using System;
using FreeCourse.Web.Models.Order;

namespace FreeCourse.Web.Models.Order//174
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }

        //ödeme geçmişi olacagı ııcn bu sınıf adree gerek yok
        public AddressViewModel Address { get; set; }
        public string BuyerId { get; set; }
        public List<OrderItemViewModel> OrderItems { get; set; }
    }
}
