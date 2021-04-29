using CarInfo;
using Microsoft.AspNetCore.Components;
using Repository.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorServer.Pages
{
    public class CarInfoBase : ComponentBase
    {

        [Inject]
        protected Carfinderinfo _carfinder { get; set; }

        protected CarModel car = new CarModel();
        protected List<string> images = new List<string>();
        protected string regInput;
        protected string errorMessage;
        protected bool displayInfo;
        protected bool IsLoading;


        protected async Task SearchForCar()
        {
            try
            {
                IsLoading = true;
                car = await _carfinder.RegLookUp(regInput);
                regInput = string.Empty;
                IsLoading = false;
            }
            catch (Exception e)
            {
                regInput = string.Empty;
                IsLoading = false;
                await renderErrorMessage();
            }

        }

        protected async Task renderErrorMessage()
        {
            errorMessage = "Found no car with this keyplate";
            StateHasChanged();
            await Task.Delay(2000);
            errorMessage = null;

        }

        protected async Task SetInput(string e)
        {
            regInput = e;
        }

    }

}
