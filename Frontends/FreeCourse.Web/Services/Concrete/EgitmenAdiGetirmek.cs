using FreeCourse.Web.Models.Identit;
using FreeCourse.Web.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace FreeCourse.Web.Services.Concrete
{
    public class EgitmenAdiGetirmek:IEgitmenAdiGetirmek
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;
        public EgitmenAdiGetirmek(IHttpClientFactory httpClientFactory, HttpClient httpClient)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient = httpClient;
        }
        //--------------------------------------------------------------------------------


        [HttpGet]
        public async Task<List<UsersViewModel>> GetAllUser()//giriş yapmadanverılerı cekemedım kullamadım
        {
            return await _httpClient.GetFromJsonAsync<List<UsersViewModel>>("api/user/getalluser");//burda userviewmodel dekı verılerı cekmek ıstıyoruz.link ise http://localhost:5001/api/user/getuser böyledır onkısmını starturda UserService cagrılgıdında otomatıkkendı tanımlıyor.
      


        }
    }
}
