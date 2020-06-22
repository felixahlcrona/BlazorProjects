using Api.Controllers;
using LazyCache;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api
{
    public class LzyCache
    {
        private readonly SystemBolaget _systemBolaget;
        IAppCache cache = new CachingService();
        public LzyCache(SystemBolaget systembolaget)
        {
            _systemBolaget = systembolaget;
        }


        public async Task<List<DrinksModel>> GetAllProductsAsync()
        {
            var cachedResults = cache.GetOrAdd("GetAllProductsAsync",
              () => _systemBolaget.GetAllProductsAsync());
            return await cachedResults;
        }
    }
}
