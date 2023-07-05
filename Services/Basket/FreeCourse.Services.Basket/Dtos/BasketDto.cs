
using System.Collections.Generic;
using System.Linq;

namespace FreeCourse.Services.Basket.Dtos
{
    public class BasketDto//54
    {
       

        public string UserId { get; set; }
        public string CourseIdForBasket { get; set; }
        public string DiscountCode { get; set; }//indirimkodu
        public int? DiscountRate { get; set; }//indirimi oranı 167 ekledık
        public bool Status { get; set; }



        public List<BasketItemDto> BasketItems { get; set; }
        public decimal TotalPrice { get => BasketItems.Sum(x => x.Price * x.Quantity); }//sepette kactane urun varsa fıyatla mıktarı kadar carpacak ve toplayacak
      
       



    }
}
