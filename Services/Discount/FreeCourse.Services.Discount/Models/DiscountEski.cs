

using System;

namespace FreeCourse.Services.Discount.Models
{
    [Dapper.Contrib.Extensions.Table("discounteski")]//bu kodu eklıyoruz daapper olsuturacagı tabloyu maplasın Discount discount buna maplasın tablo adı kucuk harflerle olan discount tur
    public class DiscountEski//69 tablo olsturuyruz
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int Rate { get; set; }
        public string Code { get; set; }

        public string CourseId { get; set; }
        public bool Status { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
