using FreeCourse.Services.Catalog.Settings.Abstract;


//23 appsettings.json dosyaısnda verıtaban abglanmaı yerı yaptık ve orda kulandıgımız ısımlerı gelıp burda ınterface tanımdadık ve aynı ısımde clasa ımpelemnt ettık sımdıde startup da kullancaz

namespace FreeCourse.Services.Catalog.Settings.Concrete
{
    public class DatabaseSettings : IDatabaseSettings
    {
        public string CourseCollectionName { get ; set ; }
        public string CategoryCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
