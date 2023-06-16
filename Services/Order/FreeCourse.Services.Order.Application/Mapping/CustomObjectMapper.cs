using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeCourse.Services.Order.Application.Mapping//90 bu sınıf mapleme ıslemını yapacak cunku classlıbrarlerın startup olmadugııcın ordan cagırmadıgımızıcın .burda halledıyoruz classlarda nasıl mapleme olacagınıda ögrenıyorzu
{
    public static class CustomObjectMapper          //bu sınıf maplama ıslemı yapıyr
    {
                                         //lazy proje ayaga kaldıgında o kalmaz ne zaman cagırısan kalka ıslemını yapr private tanımdakı  dısarıya ıstedıgımız medıdla verecez oda asagdadır adı Mapper
        private static readonly Lazy<IMapper> lazy = new Lazy<IMapper>(() =>
         {
             var config = new MapperConfiguration(cfg =>
             {
                 cfg.AddProfile<CustomMapping>();
             });
             return config.CreateMapper();
         });

        public static IMapper Mapper => lazy.Value; //burdan  cagırılıp kullanılıyor
    }

}
