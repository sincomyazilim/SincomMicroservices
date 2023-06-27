using System;

namespace FreeCourse.Services.Discount.Models
{
    [Dapper.Contrib.Extensions.Table("discount1")]
    public class Discount1
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int Rate { get; set; }
        public string Code { get; set; }
        public string CourseId { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
