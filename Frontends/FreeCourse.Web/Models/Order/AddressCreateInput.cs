namespace FreeCourse.Web.Models.Order//175
{
    public class AddressCreateInput
    {
        public string City { get; set; }//province,şehir not set olayını dısardan kımse mudahel etmesın dıe private
        public string District { get; set; }//ilçe
        public string Street { get; set; }//cadde
        public string ZipCode { get; set; }//postacodu
        public string Line { get; set; }//adres saturu


      
    }
}
