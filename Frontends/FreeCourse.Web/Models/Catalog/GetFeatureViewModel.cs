using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace FreeCourse.Web.Models.Catalog//130
{
    public class GetFeatureViewModel
    {
        [Display(Name = "Kurs Süresi")]//137
       
        public int Duration { get; set; }
    }
}
