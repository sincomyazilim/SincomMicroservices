using System.Collections.Generic;

namespace FreeCourse.Services.FakePayment.Dtos
{
    public class OrderDto//182
    {
        public OrderDto()
        {
            OrderItems = new List<OrderItemDto>();//182 bunu eklıyoruz orderItemları eklemekıstersen otamatık newlesn
        }


        public string BuyerId { get; set; }//kursu veya kursları alan kısı ıd
        public List<OrderItemDto> OrderItems { get; set; } //
        public AddressDto AddressDto { get; set; }
    }

    public class AddressDto
    {
        public string City { get; set; }//province,şehir not set olayını dısardan kımse mudahel etmesın dıe private
        public string District { get; set; }//ilçe
        public string Street { get; set; }//cadde
        public string ZipCode { get; set; }//postacodu
        public string Line { get; set; }//adres saturu
    }
    public class OrderItemDto
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price { get; set; }
    }

}


// bu classı rabıt@ ıle kullanmaya basladık butun bılgıle kuyraga a tıyoruz