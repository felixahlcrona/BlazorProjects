using EasySubFinder.Entites;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SubtitleCrawler
{
    public interface ISubtitleClass
    {
        Tuple<int, string> CalculateMostPlausibleMovieTitle(string s, string t);
        Task<MovieClass> GetMovieDetails(MovieClass e);
        Task<string> GetMoviePoster(string moviePage);
        Task<IEnumerable<MovieClass>> SubtitleSearch(string searchedMovieTitle);

    }
}