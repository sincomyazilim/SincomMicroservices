namespace FreeCourse.Web.Models//116
{
    public class ClientSettings
    {
        public Client WebClient { get; set; }
        public Client WebClientForUser { get; set; }
    }
    public class Client
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }

}
//bu sınıfat apppsetıngst tekı verılerı okumak ıcın ekleıdn serverıdentıya baglanıp ordakı config dosyasıyla eslestırılıyor