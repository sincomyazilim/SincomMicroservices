namespace FreeCourse.Web.Models.Order.Secron//174
{
    public class OrderCreatedViewModel
    {
        public int OrderId { get; set; }

        public string Error { get; set; }//hata kodo dondursekcek /175
        public bool IsSuccessful { get; set; }//basarılı olursada mesaj verdılerım 175
    }
}
