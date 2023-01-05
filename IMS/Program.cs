using IMF;
using IMF.DependencyInjection;
using IMF.Manager;
using IMF.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Security.Authentication.ExtendedProtection;
using System.Threading.Tasks;

namespace IMF
{
    internal static class Program
    {
        ///  The main entry point for the application.
        static async Task Main(string[] args)
        {
            var services = new DiServiceCollection();
            services.RegisterSingleton<MainApp>();
            var container = services.GenerateContainer();
            var mainApp = container.GetService<MainApp>();
            mainApp.StartAsync(args);
        }

    }


}