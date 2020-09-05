using System;
using System.Collections.Generic;
using System.Linq;
using AgileObjects.AgileMapper;
using System.Threading.Tasks;
using Repository.Repository;
using BlazorServer.Models;

namespace BlazorServer.Services
{
    public class SystemBolagetService : ISystemBolagetService
    {
        protected ISystemBolagetRepository _systemBolagetRepo { get; set; }

        public SystemBolagetService(ISystemBolagetRepository systemBolagetRepo)
        {
            _systemBolagetRepo = systemBolagetRepo;
        }

        public async Task<IEnumerable<SystemBolagetModel>> GetAllProductsAsync()
        {
            return Mapper.Map(await _systemBolagetRepo.GetAllProductsAsync()).ToANew<IEnumerable<SystemBolagetModel>>();
        }
    }
}
