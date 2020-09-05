using BlazorServer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorServer.Services
{
    public interface ISystemBolagetService
    {
        Task<IEnumerable<SystemBolagetModel>> GetAllProductsAsync();
    }
}