using InsiderInfo;
using Microsoft.AspNetCore.Components;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorServer.Pages
{
    public class InsiderInfoBase : ComponentBase
    {
        
        [Inject]
        protected InsiderTracker _carfinder { get; set; }
     
        public List<InsiderModel> model = new List<InsiderModel>();
        public string regInput;
        public string errorMessage;
        public bool displayInfo;
        public bool IsLoading;


        public async Task SearchForCar()
        {
            try
            {
                IsLoading = true;
                model = await _carfinder.GetInsiderData(regInput);
                regInput = "";
                IsLoading = false;
            }
            catch(Exception e)
            {
                regInput = "";
                IsLoading = false;
                await renderErrorMessage();
            }
            
        }

        public async Task renderErrorMessage()
        {
            errorMessage = "Found no car with this name";
            StateHasChanged();
            await Task.Delay(2000);
            errorMessage = null;

        }

        public async Task SetInput(string e)
        {
            regInput = e;
        }

    }




}
