using Api;
using Api.Controllers;
using AvanzaScraper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorServer.Pages
{
    public class DrinkBase : ComponentBase
    {
        public IEnumerable<DrinksModel> listOfDrinks = new List<DrinksModel>();
        public SystemBolaget _systemBolagetAPI = new SystemBolaget(new HttpClient());
        public IEnumerable<DrinksModel> listOfDrinksfilter = new List<DrinksModel>();
        public bool loading;
        public bool showMore;
        public string callbackString;
        public int Value = 10;
        public int CurrentDrinks;
        public float? CurrentValue { get; set; }

        protected override async Task OnInitializedAsync()
        {
            loading = true;
            listOfDrinks = await _systemBolagetAPI.GetAllProductsAsync();
            loading = false;
        }


        public void GetByType(string type)
        {
            listOfDrinksfilter = listOfDrinks.Where(e => e.Type == type.ToString());
            showMore = false;
        }


        public void typeAheadResultCallback(string e)
        {
            callbackString = e;
            listOfDrinksfilter = listOfDrinks.Where(e => e.ProductNameBold == callbackString);
            CurrentValue = listOfDrinksfilter.OrderBy(e => e.Price).FirstOrDefault().Price;
        }

     
    }



}
