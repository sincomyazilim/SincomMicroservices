namespace FreeCourse.Web.Models.Basket//152
{
    public class BasketItemViewModel
    {
        public int Quantity { get; set; } = 1;//miktarı 1 verıyoruuz bu proje ıcın bır kursu sadece 1 kere alabılrısın ama baska urunler satılacaksa burası =1 denmez
        public string CourseId { get; set; }
        public string CourseName { get; set; }
        public decimal Price { get; set; }//gercek fıyatı
        //indirimli kupın ıcın
        public decimal? DiscountAppliedPrice;//ındırım uygulanmıs fıyat

        public decimal GetCurrentPrice 
        {
            get => DiscountAppliedPrice!=null?DiscountAppliedPrice.Value: Price;//indirim uygulanmıssa ındırımlı fıyat yok gercek fıyat
            //get => DiscountAppliedPrice!=null?DiscountAppliedPrice.Value: DiscountAppliedPrice.Value;//indirim uygulanmıssa ındırımlı fıyat yok gercek fıyat
                
        }


        public void AppiledDiscount(decimal discountPrice)//indirimli mıktar
        {
            DiscountAppliedPrice = discountPrice;
        }
    }
}
