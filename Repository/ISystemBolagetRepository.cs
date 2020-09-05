using Repository.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public interface ISystemBolagetRepository
    {
        Task<List<SystemBolagetEntity>> GetAllProductsAsync();
    }
}