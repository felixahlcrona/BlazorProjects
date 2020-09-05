using HtmlAgilityPack;
using Repository.Entities;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public interface ISubtitleFinderRepository
    {
        Task<HtmlDocument> GetAllSubtitlesForMovie(string movieUrl);
        Task<HtmlDocument> GetMovieDetails(SubtitleFinderEntity e);
        Task<HtmlDocument> GetMoviePoster(string moviePage);
        Task<HtmlDocument> MostPlauisableMovie(string searchQuery);
    }
}