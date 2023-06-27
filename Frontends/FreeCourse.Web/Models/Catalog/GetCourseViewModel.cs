using System;

namespace FreeCourse.Web.Models.Catalog//130
{
    public class GetCourseViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ShortDescription  
        { get => Description.Length > 100 ? Description.Substring(0, 100) + "..." : Description;//148
            //gelen gercek descrıpton 100 karekterden buyukse 100 snıarla degılse tamamnı getır
        }
        public decimal Price { get; set; }
        public string UserId { get; set; }
        public string Picture { get; set; }
        public string StockPictureUrl { get; set; }

        public DateTime CreatedTime { get; set; }

        //---------------------------------------dersler teke tek ındırım uygulamlı-------------------------








        //----------------------------------------------------------------------------------------------------

        public GetFeatureViewModel Feature { get; set; }
        public string CategoryId { get; set; }
        public GetCategoryViewModel Category { get; set; }

    }
}
