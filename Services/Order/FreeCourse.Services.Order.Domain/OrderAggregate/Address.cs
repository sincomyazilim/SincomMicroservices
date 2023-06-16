using FreeCourse.Services.Order.Domain.Core.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeCourse.Services.Order.Domain.OrderAggregate//87
{// normalde domain kangı orm kullandıgını bıllemmeısgerekır zarurıs durumlar harıc onun ıcın [Owned] sozcugunu gelıp bu sınıfta tanımlarsak ef core orm  oldugunuanlayacagı ıcın hem bagımlılık olacak onun ıcın bız [owned] application katmanındada dbcnntext sınıfında tanımlayacaz

    //[Owned] tanımla bu sekılde olacaktı bagmlılk olmasını ıstemıyoruz bunu dbcontextte tanımlıyoruz
    public class Addres:ValueObject
    {
        public string City { get;private set; }//province,şehir not set olayını dısardan kımse mudahel etmesın dıe private
        public string District { get; private set; }//ilçe
        public string Street { get; private set; }//cadde
        public string ZipCode { get; private set; }//postacodu
        public string Line { get; private set; }//adres saturu

        public Addres(string city, string district, string street, string zipCode, string line)//contrtor ekledık
        {
            City = city;
            District = district;
            Street = street;
            ZipCode = zipCode;
            Line = line;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return City; 
            yield return District; 
            yield return Street; 
            yield return ZipCode; 
            yield return Line;
        }
    }
}
