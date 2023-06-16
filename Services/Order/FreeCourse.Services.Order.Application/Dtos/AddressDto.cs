using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeCourse.Services.Order.Application.Dtos//89
{
    public class AddressDto
    {
        public string City { get;  set; }//province,şehir not set olayını dısardan kımse mudahel etmesın dıe private
        public string District { get;  set; }//ilçe
        public string Street { get;  set; }//cadde
        public string ZipCode { get;  set; }//postacodu
        public string Line { get;  set; }//adres saturu
    }
}
