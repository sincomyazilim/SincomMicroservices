﻿using FreeCourse.Web.Models.FakePaymnet;
using FreeCourse.Web.Services.Abstract;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace FreeCourse.Web.Services.Concrete//171
{
    public class FakePaymentService : IFakePaymentService
    {

        private readonly HttpClient _httpClient;

        public FakePaymentService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        //-----------------------------------------------------------------
        public async Task<bool> ReceivePayment(FakePaymentInfoInput input)//172
        {
            var response = await _httpClient.PostAsJsonAsync<FakePaymentInfoInput>("fakepayment", input);
            return response.IsSuccessStatusCode;
        }
    }
}
