namespace FreeCourse.Services.FakePayment.Dtos
{
    public class FakePaymentAsekronDto
    {
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public string Expiration { get; set; }//karttta 4 hanelı tarıh ay yıl
        public string CVV { get; set; }//arka yuzdkeı 3 3 hane
        public decimal TotalPrice { get; set; }//sepettekı toplam fıyat


        public OrderDto Order { get; set; }// 183 ekledık 185 te web kısmıdna order kısmına ekledık
    }
}
//hoca FakePaymentDto uzerınden eklemek yaparak gıttı ben ayrı sonıf tanımladım adına FakePaymentAsekronDto