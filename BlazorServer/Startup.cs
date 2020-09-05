//using BlazorServer.Data;
using Api;
using Api.Controllers;
using BlazorServer.Pages;
using CarInfo;
using EasySubFinder.Entites;
using LazyCache;
using Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SubtitleCrawler;
using Syncfusion.Blazor;
using System.Net.Http;
using Repository.Repository;
using BlazorServer.Services;

namespace BlazorServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddSyncfusionBlazor();
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MjcxMTM4QDMxMzgyZTMxMmUzMEUwVHZVTWR3RFBYSnJQaXk2eUNoTWROWURNaWhoL2dNTHZhTEQ4azFJbWc9");
            services.AddServerSideBlazor().AddCircuitOptions(options => { options.DetailedErrors = true; });
            services.AddTransient<HttpClient>();
            services.AddTransient<IAppCache, CachingService>();
            services.AddTransient<Carfinderinfo>();
            //services.AddSingleton<LzyCache>();

            //SystemBolaget
            services.AddTransient<ISystemBolagetService,SystemBolagetService>();
            services.AddTransient<ISystemBolagetRepository,SystemBolagetRepository>();

            //SubtitleFinder
            services.AddTransient<ISubtitleFinderService, SubtitleFinderService>();
            services.AddTransient<ISubtitleFinderRepository, SubtitleFinderRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
