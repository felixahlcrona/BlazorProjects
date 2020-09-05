using Api.Controllers;
using EasySubFinder.Entites;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using SubtitleCrawler;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using BlazorServer.Services;
using BlazorServer.Models;

namespace BlazorServer.Pages
{
    public class SubtitleFinderBase : ComponentBase
    {

        [Inject]
        protected ISubtitleFinderService _subtitleFinderService { get; set; }
        public SubtitleFinderModel movie = new SubtitleFinderModel();
        public IEnumerable<SubtitleFinderModel> movieList = new List<SubtitleFinderModel>();
        public SubtitleFinderModel selectedMovieDetails = new SubtitleFinderModel();

        public string movieInput;
        public string errorMessage;
        public bool displayInfo;
        public bool loading;
        public string language = "English";
        public bool displayExample;
        public int Counter = 1;
        public bool newSearch;

        public List<string> exampleMovies = new List<string>()
        {
            "Tropic.Thunder.Unrated.2008.BluRay.DTS.x264.dxva-EuReKA",
            "Jojo.Rabbit.2019.DVDScr.XVID.AC3.Hive-CM8[TGx]",
            "John.Wick.3.2019.HDRip.XviD.AC3-EVO",
            "Extraction.2020.HDRip.XviD.AC3-EVO[TGx]",
            "Ad.Astra.2019.HDRip.XviD.AC3-EVO[TGx]",
            "Knives.Out.2019.DVDScr.XVID.AC3.Hive-CM8",
            "Ready.or.Not.2019.DVDRip.XviD.AC3-EVO[TGx]",
            "Scoob.2020.HDRip.XviD.AC3-EVO[TGx]",
            "Mortal.Kombat.Legends.Scorpions.Revenge.2020.HDRip.XviD.AC3-EVO"
        };


        public async Task SearchForMovie()
        {

            try
            {
                loading = true;
                movieList = await _subtitleFinderService.SubtitleSearch(movieInput);
                movieInput = "";
                loading = false;
                newSearch = false;
            }
            catch (Exception e)
            {
                movieInput = "";
                loading = false;
                language = "English";
                await renderErrorMessage();
            }

        }

        public async Task renderErrorMessage()
        {
            errorMessage = "Found no subtitles with this name";
            StateHasChanged();
            await Task.Delay(2000);
            errorMessage = null;

        }
        public async Task GetSelectedMovieDetails(SubtitleFinderModel movie)
        {
            try
            {

                selectedMovieDetails = await _subtitleFinderService.GetMovieDetails(movie);
            }
            catch (Exception e)
            {

            }
        }

        public async Task ExampleSearch(string e)
        {
            Random rnd = new Random();
            int random = rnd.Next(exampleMovies.Count);
            movieInput = (string)exampleMovies[random];

            await SearchForMovie();
        }
        public async Task SetInput(string e)
        {
            movieInput = e;

        }

    }




}
