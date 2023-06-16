using FreeCourse.Services.Order.Domain.Core.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeCourse.Services.Order.Domain.OrderAggregate//87
{
    public class OrderItem:Entity
    {
        public string ProductId { get;private set; }
        public string ProductName { get; private set; }
        public string PictureUrl { get; private set; }
        public decimal Price { get; private set; }

        //public int OrderId { get; set; }//tanımlamıyorz ef core bunu kendısı anlıyor bu tıplere shadow denır

        public OrderItem()
        {
            
        }
        public OrderItem(string productId, string productName, string pictureUrl, decimal price)//consture ekledık
        {
            ProductId = productId;
            ProductName = productName;
            PictureUrl = pictureUrl;
            Price = price;
        }

        public void UpdateOrderItem(string productName,string pictureUrl, decimal price)//update etmek ıcın medıt
        {
            ProductName = productName;
            PictureUrl = pictureUrl;
            Price = price;
        }
    }
}
