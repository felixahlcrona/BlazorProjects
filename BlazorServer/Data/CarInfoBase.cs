using CarInfo;
using Microsoft.AspNetCore.Components;

using System;
using System.Threading.Tasks;

namespace BlazorServer.Pages
{
    public class CarInfoBase : ComponentBase
    {
        
        [Inject]
        protected Carfinderinfo _carfinder { get; set; }

        public CarModel car = new CarModel();
        public string regInput;
        public string errorMessage;
        public bool displayInfo;
        public bool IsLoading;


        public async Task SearchForMovie()
        {
            try
            {
                IsLoading = true;
                car = await _carfinder.RegLookUp(regInput);
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
