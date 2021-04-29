using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public interface IGoogleImagesRepository
    {
        Task<List<string>> GetGoogleImages(string query);
    }
}