using Microsoft.AspNetCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace IMF
{
    public class MainApp
    {
        public async Task StartAsync(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }
        private IWebHostBuilder CreateWebHostBuilder(string[] args) =>
           WebHost.CreateDefaultBuilder(args)
           .UseStartup<Startup>();
  
    }
}