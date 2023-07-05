using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace FreeCourse.Web.Models.Basket//152
{
    public class BasketViewModel
    {
        public BasketViewModel()
        {
            _basketItems = new List<BasketItemViewModel>();

        }





        public string UserId { get; set; }
        public string CourseIdForBasket { get; set; }

        public string DiscountCode { get; set; }//indirimkodu
        public int? DiscountRate { get; set; } //indirimi oranı
        public bool Status { get; set; }

        private List<BasketItemViewModel> _basketItems;


        public List<BasketItemViewModel> BasketItems//sepet içindekı ıtemlerın prceları ındrım uygunamsısa uygulyor
        {
            get
            {//ornek kurs 100 tl prce 100tl,indirim oranı %10 oda 10 yapar yanı discountPrice=10

                _basketItems.ForEach(b =>
                {
                    if (HasDiscount)
                    {

                            var discountPrice = b.Price * ((decimal)DiscountRate.Value / 100);//indırım mıktarı yanı 10tl
                            b.AppiledDiscount(Math.Round(b.Price - discountPrice, 2));//ilandakı fıyattan ınırm mıktarını cıkarıp ,yuvarla sonucu vırgulden sonra 2 basamak al --- price-discount=100-10=90.00 tl

                    }
                   



                });

                return _basketItems;


            }
            set
            {
                _basketItems = value;
            }
        }


        public decimal TotalPrice
        { //sepette kactane urun varsa fiyatın son durumuna göre  carpacak ve toplayacak
            get => _basketItems.Sum(x => x.GetCurrentPrice * x.Quantity);


        }






        // dıcount mıcroservıdeten indirim code gelıyormu orden code ve rate yanıoran gelıyor  ornek code abc rate %10
        public bool HasDiscount //indirim uygulanmısmı
        {
            get => !string.IsNullOrEmpty(DiscountCode) && DiscountRate.HasValue;//&& !string.IsNullOrEmpty(CourseIdForBasket);
            /*get => !string.IsNullOrEmpty(DiscountCode);*/ //kod varmı yokmu varsa ındırımuygulanmıs yoksa uygulanmamıs
        }


        public void ApplyDiscount(string courseId, string code, int rate, bool status)
        {
            CourseIdForBasket = courseId;
            DiscountCode = code;
            DiscountRate = rate;
            Status = status;
            //-----------------------------


        }
        public void CanselDiscount()//burda ındırım konud ıptal etmekıcın metot yazdık guncelıyoruzo kısmı 167
        {
            DiscountCode = null;
            DiscountRate = null;
            CourseIdForBasket = null;
        }
    }
}
