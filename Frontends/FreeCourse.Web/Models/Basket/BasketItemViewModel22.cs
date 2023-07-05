using FreeCourse.Web.Models.Catalog;
using FreeCourse.Web.Models.DiscountCourse;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace FreeCourse.Web.Models.Basket//152
{
    public class BasketItemViewModel22
    {
        public BasketItemViewModel22()
        {
            _degerler = new List<SadeceTekDers>();

        }



        public int Quantity { get; set; } = 1;//miktarı 1 verıyoruuz bu proje ıcın bır kursu sadece 1 kere alabılrısın ama baska urunler satılacaksa burası =1 denmez
        public string CourseId { get; set; }
        public string CourseName { get; set; }
        public bool Status { get; set; }
        public decimal Price { get; set; }//gercek fıyatı
        public decimal? DiscountAppliedPrice;//ındırım uygulanmıs fıyat

        public decimal GetCurrentPrice
        {
            get => DiscountAppliedPrice != null ? DiscountAppliedPrice.Value : Price;//indirim uygulanmıssa ındırımlı fıyat yok gercek fıyat
            //get => DiscountAppliedPrice!=null?DiscountAppliedPrice.Value: DiscountAppliedPrice.Value;//indirim uygulanmıssa ındırımlı fıyat yok gercek fıyat

        }


        public void AppiledDiscount(decimal discountPrice)//indirimli mıktar
        {
            DiscountAppliedPrice = discountPrice;
        }


        //-----------------------------------------------------------------------------------------
        private string CourseIdForBasket {get;set;}
        private string DiscountCode { get; set; }
        private int? DiscountRate { get; set; }

        public List<SadeceTekDers> _degerler;
        public List<SadeceTekDers> Degerler
        {
            get
            {
                if (HasDiscount)
                {
                    _degerler.Add(deger);
                }

          

                return _degerler;
            }
            set { _degerler = value; }
        }
        SadeceTekDers deger = new SadeceTekDers();
        public void ApplyDiscount(string courseId, string code, int rate)
        {
            CourseIdForBasket = courseId;
            DiscountCode = code;
            DiscountRate = rate;

            deger =new SadeceTekDers()
            {
                CourseId = courseId,
                CourseCode = code,
                CourseRate = rate
            };

        }
        public bool HasDiscount //indirim uygulanmısmı
        {
            get => !string.IsNullOrEmpty(DiscountCode) && DiscountRate.HasValue && Status == false;//&& !string.IsNullOrEmpty(CourseIdForBasket);
            /*get => !string.IsNullOrEmpty(DiscountCode);*/ //kod varmı yokmu varsa ındırımuygulanmıs yoksa uygulanmamıs
        }
    }

}