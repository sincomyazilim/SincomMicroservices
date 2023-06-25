using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace FreeCourse.Web.Models.Catalog//130
{
    public class UpdateCourseInput
    {
        public string Id { get; set; }
        [Display(Name = "Kurs Adı")]//137 eklendı
       
        public string Name { get; set; }
        [Display(Name = "Açıklama ")]
   
        public string Description { get; set; }
        [Display(Name = "Kurs Fiyatı")]
       
        public decimal Price { get; set; }
        public string StockPictureUrl { get; set; }
        public string UserId { get; set; }
        public string Picture { get; set; }

        public GetFeatureViewModel Feature { get; set; }
        [Display(Name = "Kurs Kategori")]
       
        public string CategoryId { get; set; }


        [Display(Name = "Kurs Resimi")]

        public IFormFile PhotoFormFile { get; set; }//147 ekledık
    }
}
