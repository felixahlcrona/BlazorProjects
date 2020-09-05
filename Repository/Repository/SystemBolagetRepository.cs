using Repository.Entities;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{


    public class SystemBolagetRepository : ISystemBolagetRepository
    {
        public static string AzureDatabase = Properties.Resource.AzureDatabase;

        private readonly HttpClient _clientFactory;
        private List<SystemBolagetEntity> list;
        private string errorResponse;

        public SystemBolagetRepository(HttpClient clientFactory)
        {
            _clientFactory = clientFactory;
        }


        public async Task<List<SystemBolagetEntity>> GetAllProductsAsync()
        {

            var request = new HttpRequestMessage(HttpMethod.Get, "https://api-extern.systembolaget.se/product/v1/product");
            _clientFactory.DefaultRequestHeaders.Clear();
            _clientFactory.DefaultRequestHeaders.Add("Host", "api-extern.systembolaget.se");
            _clientFactory.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "7bc3f02b4c574e74aeb2cbff9a3c1258");
            HttpResponseMessage response = await _clientFactory.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                list = await response.Content.ReadFromJsonAsync<List<SystemBolagetEntity>>();
            }

            return list;

        }
    }
}
