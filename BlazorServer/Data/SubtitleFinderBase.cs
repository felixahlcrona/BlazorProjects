using Api.Controllers;
using EasySubFinder.Entites;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using SubtitleCrawler;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorServer.Pages
{
    public class SubtitleFinderBase : ComponentBase
    {

        public MovieClass movie = new MovieClass();
        public SubtitleClass subtitle = new SubtitleClass();
        public IEnumerable<MovieClass> movieList = new List<MovieClass>();
        public MovieClass selectedMovieDetails = new MovieClass();
        public string movieInput;
        public string errorMessage;
        public bool displayInfo;
        public bool loading;
        public string language="English";
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
            "Captain Marvel 2019 NEW HD-TS X264 AC3-SeeHD",
            "Ready.or.Not.2019.DVDRip.XviD.AC3-EVO[TGx]",
            "Scoob.2020.HDRip.XviD.AC3-EVO[TGx]",
            "Mortal.Kombat.Legends.Scorpions.Revenge.2020.HDRip.XviD.AC3-EVO"
        };

 
 



        public async Task SearchForMovie()
        {
            try
            {
                loading = true;
                movieList = await subtitle.SubtitleSearch(movieInput);
                movieInput = "";
                loading = false;
                newSearch = false;
            }
            catch(Exception e)
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
        public async Task GetSelectedMovieDetails(MovieClass movie)
        {
            try
            {

            selectedMovieDetails = await subtitle.GetMovieDetails(movie);
            }
            catch (Exception e)
            {

            }
        }

        public async Task ExampleSearch(string e)
        {
            movieInput = e;
            await SearchForMovie();

        }
        public async Task SetInput(string e)
        {
            movieInput = e;

        }
        //public async Task ClickHandlerEX(string newMessage)
        //{
        //    movieInput = newMessage;
        //    await SearchForMovie();
        //}

    }




}
