

using System;

namespace FreeCourse.Services.Discount.Models
{
    [Dapper.Contrib.Extensions.Table("discount")]//bu kodu eklıyoruz daapper olsuturacagı tabloyu maplasın Discount discount buna maplasın tablo adı kucuk harflerle olan discount tur
    public class Discount//69 tablo olsturuyruz
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int Rate { get; set; }
        public string Code { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
