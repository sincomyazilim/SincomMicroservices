using FreeCourse.Services.Catalog.Services.Abstract;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCourse.Services.Catalog
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //CreateHostBuilder(args).Build().Run();//bunu ýptal ettýk genýslettýk ayaga kalkarken otomatýk ýkýtane categorý ekleýsn 107
            var host=CreateHostBuilder(args).Build();
            using (var scope=host.Services.CreateScope())//107 catalog kaymaný ayaga kalkerken verýtaban otomatýk verý ekleyecek eger data yoksa ekle 
            {
                var servicesProvider = scope.ServiceProvider;
                var categoryService=servicesProvider.GetRequiredService<ICategoryService>();
                if (!categoryService.GetAllAsync().Result.Data.Any())
                {
                    categoryService.CreateCategoryAsync(new Dtos.CategorieDto.CategoryCreateDto { Name="Asp.net Core Kursu"}).Wait();
                    categoryService.CreateCategoryAsync(new Dtos.CategorieDto.CategoryCreateDto { Name="Asp.net Core Api Kursu"}).Wait();
                }
            }
           host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
