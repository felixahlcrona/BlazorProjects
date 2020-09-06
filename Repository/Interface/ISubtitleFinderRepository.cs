using HtmlAgilityPack;
using Repository.Entities;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public interface ISubtitleFinderRepository
    {
        Task<HtmlDocument> ExecuteGetRequest(string url);
        Task<HtmlDocument> ExecutePostRequest(string url);
    }
}