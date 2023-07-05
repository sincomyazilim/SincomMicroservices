namespace FreeCourse.Services.FakePayment.Dtos//170
{
    public class FakePaymentDto
    {
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public string Expiration { get; set; }//karttta 4 hanelı tarıh ay yıl
        public string CVV { get; set; }//arka yuzdkeı 3 3 hane
        public decimal TotalPrice { get; set; }//sepettekı toplam fıyat


        
    }
}
