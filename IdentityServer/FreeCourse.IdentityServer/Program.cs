// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using FreeCourse.IdentityServer.Data;
using FreeCourse.IdentityServer.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.Linq;

namespace FreeCourse.IdentityServer
{
    public class Program
    {
        public static int Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
                .Enrich.FromLogContext()
                // uncomment to write to Azure diagnostics stream
                //.WriteTo.File(
                //    @"D:\home\LogFiles\Application\identityserver.txt",
                //    fileSizeLimitBytes: 1_000_000,
                //    rollOnFileSizeLimit: true,
                //    shared: true,
                //    flushToDiskInterval: TimeSpan.FromSeconds(1))
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Code)
                .CreateLogger();

            try
            {
               // CreateHostBuilder(args).Build().Run();//sildık

                var host = CreateHostBuilder(args).Build();

                //burda usıng uzerınden startupdakı ApplicationDbContext ulasıyoruz ve ordadan otomatık mıfgretıon yapyoruz verıtanı yoksa kurar mıgratoın yoksa olusturu varsa son halını update eder otomatıklatırıyoruz proje ayagı kalkıtıgında ve usermanger uzerınden de egerkı user tablosunda kullancı yoksa otomatık user ekle
                using (var scope=host.Services.CreateScope())
                {
                    var serviceProvider = scope.ServiceProvider;
                    var applicationDbcontext = serviceProvider.GetRequiredService<ApplicationDbContext>();
                    applicationDbcontext.Database.Migrate();//otomatık mıratoın yaapr 

                    var userManager=serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();//aya kalkarken kullanıcı eklenıyr
                    if (!userManager.Users.Any())
                    {
                        userManager.CreateAsync(new ApplicationUser
                        {
                            UserName = "ismail",
                            Email = "ismailsincar@gmail.com",
                            Sehir = "mardin"
                        }, "iS47????").Wait();//burda userbilgisi ve şifre verıyoruz wait() metodu async yapmammak ıcın sade bu kısmı asankron yapıyoruz
                        
                    }
                }




                Log.Information("Starting host...");
                host.Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly.");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}