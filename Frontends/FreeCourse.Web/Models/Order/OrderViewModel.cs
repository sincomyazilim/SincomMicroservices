using System.Collections.Generic;
using System;

namespace FreeCourse.Web.Models.Order//174
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }

        //ödeme geçmişi olacagı ııcn bu sınıf adree gerek yok
       // public AddressDto Address { get; set; }
        public string BuyerId { get; set; }
        private List<OrderItemViewModel> OrderItems { get; set; }
    }
}
