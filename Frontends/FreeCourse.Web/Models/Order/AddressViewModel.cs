namespace FreeCourse.Web.Models.Order
{
    public class AddressViewModel
    {
        public string City { get; set; }//province,şehir not set olayını dısardan kımse mudahel etmesın dıe private
        public string District { get; set; }//ilçe
        public string Street { get; set; }//cadde
        public string ZipCode { get; set; }//postacodu
        public string Line { get; set; }//adres saturu
    }
}
