using Api;
using Api.Controllers;
using AvanzaScraper;
using BlazorServer.Models;
using BlazorServer.Services;
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
        //[Inject]
        //protected LzyCache _lzyCache { get; set; }

        [Inject]
        protected ISystemBolagetService _systemBolagetRepo { get; set; }

        public IEnumerable<SystemBolagetModel> listOfDrinks = new List<SystemBolagetModel>();
        public IEnumerable<SystemBolagetModel> listOfDrinksfilter = new List<SystemBolagetModel>();

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
            listOfDrinks = await _systemBolagetRepo.GetAllProductsAsync();
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
