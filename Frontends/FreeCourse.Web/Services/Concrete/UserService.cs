using FreeCourse.Shared.Dtos;
using FreeCourse.Web.Models;
using FreeCourse.Web.Models.Catalog;
using FreeCourse.Web.Models.Identit;
using FreeCourse.Web.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace FreeCourse.Web.Services.Concrete//123
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpClientFactory _httpClientFactory;
        public UserService(HttpClient httpClient, IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClient;
            _httpClientFactory = httpClientFactory;
        }
        //--------------------------------------------------
        public async Task<UserViewModel> GetUser()//124
        {
           

            //token ekle ıstege gönder
            return await _httpClient.GetFromJsonAsync<UserViewModel>("/api/user/getuser");//burda userviewmodel dekı verılerı cekmek ıstıyoruz.link ise http://localhost:5001/api/user/getuser böyledır onkısmını starturda UserService cagrılgıdında otomatıkkendı tanımlıyor.
        }

       
    }
}
