using System.Collections.Generic;

namespace FreeCourse.Services.Basket.Dtos//54
{
    public class BasketItemDto
    {
        public int Quantity { get; set; }//miktar
        public string CourseId { get; set; }
        public string CourseName { get; set; }
        public bool Status { get; set; }
        public decimal Price { get; set; }

      
        public List<SadeceTekDersDto> Degerler { get; set; }

    }
}
