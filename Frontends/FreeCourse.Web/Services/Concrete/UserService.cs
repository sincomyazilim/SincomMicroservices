using FreeCourse.Web.Models;
using FreeCourse.Web.Services.Abstract;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace FreeCourse.Web.Services.Concrete//123
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        //--------------------------------------------------
        public async Task<UserViewModel> GetUser()//124
        {
            //token

            //token ekle ıstege gönder
            return await _httpClient.GetFromJsonAsync<UserViewModel>("/api/user/getuser");//burda userviewmodel dekı verılerı cekmek ıstıyoruz.link ise http://localhost:5001/api/user/getuser böyledır onkısmını starturda UserService cagrılgıdında otomatıkkendı tanımlıyor.
        }
    }
}
