namespace FreeCourse.Web.Models.Discount
{
    public class DiscountForCourseViewModel
    {
        public string UserId { get; set; }
        public int Rate { get; set; }
        public string Code { get; set; }
        public string CourseId { get; set; }
    }
}
