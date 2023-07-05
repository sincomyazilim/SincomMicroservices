using System;
using System.Collections.Generic;
using System.Text;

namespace FreeCourse.Shared.Messages//181
{
    public class CreateOrderMessageCommand
    {

        public CreateOrderMessageCommand()
        {
            OrderItems = new List<OrderItem>(); 
        }
        public string BuyerId { get; set; }
        public List<OrderItem> OrderItems { get; set; }

        public string City { get; set; }//province,şehir not set olayını dısardan kımse mudahel etmesın dıe private
        public string District { get; set; }//ilçe
        public string Street { get; set; }//cadde
        public string ZipCode { get; set; }//postacodu
        public string Line { get; set; }//adres saturu
    }
    public class OrderItem
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price { get; set; }
    }
}
