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

            var request = new HttpRequestMessage(HttpMethod.Get, "https://api-extern.systembolaget.se/site/v1/site");
            _clientFactory.DefaultRequestHeaders.Clear();
            _clientFactory.DefaultRequestHeaders.Add("Host", "api-extern.systembolaget.se");
            _clientFactory.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "c2caba4c1b6b471b8fdc444f40488674");
            HttpResponseMessage response = await _clientFactory.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                list = await response.Content.ReadFromJsonAsync<List<SystemBolagetEntity>>();
            }

            return list;

        }
    }
}
