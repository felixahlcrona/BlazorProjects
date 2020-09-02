using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using LazyCache;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SystemBolaget : ControllerBase
    {
        private readonly HttpClient _clientFactory;
        private List<DrinksModel> drinks;
        private string errorResponse;
        public SystemBolaget(HttpClient clientFactory)
        {
            _clientFactory = clientFactory;
        }


        [HttpGet]
        public async Task<List<DrinksModel>> GetAllProductsAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://api-extern.systembolaget.se/product/v1/product");
            _clientFactory.DefaultRequestHeaders.Clear();
            _clientFactory.DefaultRequestHeaders.Add("Host", "api-extern.systembolaget.se");
            _clientFactory.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "7bc3f02b4c574e74aeb2cbff9a3c1258");
            HttpResponseMessage response = await _clientFactory.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                
                drinks = await response.Content.ReadFromJsonAsync<List<DrinksModel>>();
            }
            else
            {
                errorResponse = $"{response.ReasonPhrase}";
            }

            return drinks;
        }
    }
}
