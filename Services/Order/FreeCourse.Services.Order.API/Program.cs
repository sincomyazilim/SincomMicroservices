using FreeCourse.Services.Order.Infrastructure.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCourse.Services.Order.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //CreateHostBuilder(args).Build().Run;//213 bu kodu ýptal edýpasagýdaký sekle cevýrýyorzu order tablosu otomatýk mýgratoý nyapýp eklenemsý ýcýn eklýyse eklemez degýlse ekler
            var host = CreateHostBuilder(args).Build();
            using (var scope=host.Services.CreateScope())
            {
                var serviceProvider= scope.ServiceProvider;
                var orderDbContext=serviceProvider.GetRequiredService<OrderDbContext>();
                orderDbContext.Database.Migrate();
            }
                host.Run();
            //213--------------------------------------------------------------------------------------
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
