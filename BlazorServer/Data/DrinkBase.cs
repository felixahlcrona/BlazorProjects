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
        private LzyCache _lzyCache = new LzyCache(new SystemBolaget(new HttpClient()));
        public IEnumerable<DrinksModel> listOfDrinksfilter = new List<DrinksModel>();

        public bool isLoading;
        public bool isShowCategory;
        public bool isDialogOpen = false;
        public bool isPriceFiltering;
        public string callbackString;
        public int Value = 10;
        public int CurrentDrinks;
        public string dialogId;
        public float? CurrentValue { get; set; }


        public void OpenDialog(string e)
        {
            dialogId = e;
            isDialogOpen = true;
        }
        protected override async Task OnInitializedAsync()
        {
            isLoading = true;
            listOfDrinks = await _lzyCache.GetAllProductsAsync();
            isLoading = false;
        }


        public void GetByType(string type)
        {
            listOfDrinksfilter = listOfDrinks.Where(e => e.Type == type.ToString());
            CurrentValue = listOfDrinksfilter.OrderBy(e => e.Price).FirstOrDefault().Price;
            isShowCategory = false;
            isPriceFiltering = true;
        }


        public void TypeAheadResultCallback(string e)
        {
            callbackString = e;
            listOfDrinksfilter = listOfDrinks.Where(e => e.ProductNameBold == callbackString);
            CurrentValue = listOfDrinksfilter.OrderBy(e => e.Price).FirstOrDefault().Price;
            isPriceFiltering = false;
        }

        
       

     
    }



}
