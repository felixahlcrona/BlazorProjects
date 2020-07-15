using Api.Controllers;
using LazyCache;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api
{
    public class LzyCache
    {
        private readonly SystemBolaget _systemBolaget;
        private readonly IAppCache _cache;
        public LzyCache(SystemBolaget systembolaget, IAppCache cache)
        {
            _systemBolaget = systembolaget;
            _cache = cache;
        }


        public async Task<List<DrinksModel>> GetAllProductsAsync()
        {
            var cachedResults = _cache.GetOrAdd("GetAllProductsAsync",
              () => _systemBolaget.GetAllProductsAsync());
            return await cachedResults;
        }
    }
}
