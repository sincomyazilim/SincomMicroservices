using System.Collections.Generic;
using System.Linq;

namespace FreeCourse.Services.Basket.Dtos
{
    public class BasketDto//54
    {
        public string UserId { get; set; }
        public string DiscountCode { get; set; }//indirimkodu
        public List<BasketItemDto> BasketItems { get; set; }
        public decimal TotalPrice { get => BasketItems.Sum(x => x.Price * x.Quantity); }//sepette kactane urun varsa fıyatla mıktarı kadar carpacak ve toplayacak

    }
}
