namespace FreeCourse.Web.Models//115
{
    public class ServiceApiSettings
    {
        public string IdentityBaseUrl { get; set; }
        public string GatewayBaseUrl { get; set; }
        public string PhotoStockUrl { get; set; }
        public ServiceApi Catalog { get; set; }//132 mvc projednen gateyaw baglan orda Indettıserver yetkın varmı yookmu,varsa  Cataloga baglan  
        public ServiceApi PhotoStock { get; set; }//143 
        public ServiceApi Basket { get; set; }//154
        public ServiceApi Discount { get; set; }//162
        public ServiceApi FakePayment { get; set; }//171
        public ServiceApi Order { get; set; }//174
    }

    public class ServiceApi//132
    {
        public string Path { get; set; }//132 cataloga  baglanmak ve bu yolu burdan al
    }



}
//bu sınıf appsetıng dosyaındkı verılerı okumak ıcın ekleıdn