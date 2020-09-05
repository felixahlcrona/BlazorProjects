using BlazorServer.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorServer.Services
{
    public interface ISubtitleFinderService
    {
        Tuple<int, string> CalculateMostPlausibleMovieTitle(string s, string t);
        Task<string> GetMoviePoster(string moviePage);
        Task<IEnumerable<SubtitleFinderModel>> SubtitleSearch(string searchedMovieTitle);
        Task<SubtitleFinderModel> GetMovieDetails(SubtitleFinderModel movie);
    }
}