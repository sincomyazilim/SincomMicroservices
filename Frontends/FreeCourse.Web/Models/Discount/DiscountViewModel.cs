using System;

namespace FreeCourse.Web.Models.Discount//161
{
    public class DiscountViewModel
    {
        
        public string UserId { get; set; }
        public int Rate { get; set; }
        public string Code { get; set; }
        public string CourseId { get; set; }
        public bool Status { get; set; }
        
    }
}
