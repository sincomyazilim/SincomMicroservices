using FreeCourse.Web.Models.Order;

namespace FreeCourse.Web.Models.FakePaymnet
{
    public class FakePaymenAsenkrontInfoInput//185 asenron ıcın olusturduk
    {
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public string Expiration { get; set; }//karttta 4 hanelı tarıh ay yıl
        public string CVV { get; set; }//arka yuzdkeı 3 3 hane
        public decimal TotalPrice { get; set; }//sepettekı toplam fıyat



        //ASENRON ICIN BUNU EKLIYRUZ  185 ekledık aynı kısım ıcın facepayment projesınde
        public OrderCreateInput Order { get; set; }
    }
}
// ben farklı classs tanmladım sekron ve asenron farklı klaclar olsun ıstedım uerıne eklemek ıstemeım