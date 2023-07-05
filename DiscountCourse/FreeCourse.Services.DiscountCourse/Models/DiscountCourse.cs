using System;

namespace FreeCourse.Services.DiscountCourse.Models
{
    [Dapper.Contrib.Extensions.Table("discountcourse")]
    public class DiscountCourse
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int Rate { get; set; }
        public string Code { get; set; }
        public string CourseId { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
