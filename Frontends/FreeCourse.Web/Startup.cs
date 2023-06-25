using FluentValidation.AspNetCore;
using FreeCourse.Web.Extensions;
using FreeCourse.Web.Handler;
using FreeCourse.Web.Helpers;
using FreeCourse.Web.Models;
using FreeCourse.Web.Services.Abstract;
using FreeCourse.Web.Services.Concrete;
using FreeCourse.Web.Validator;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace FreeCourse.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

       
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ServiceApiSettings>(Configuration.GetSection("ServiceApiSettings"));//115 burda appsett�ng dosyas�ndan ver� okuyoruz ServiceApiSettings bu tag alt�ndak�nden. oda model klasorunde ServiceApiSettings s�n�f�dn 
            services.Configure<ClientSettings>(Configuration.GetSection("ClientSettings"));//116 burda appsett�ng dosyas�ndan ver� okuyoruz ClientSettings bu tag alt�ndak�nden. oda model klasorunde ClientSettings s�n�f�dn 
         
            services.AddHttpContextAccessor();//119 UserId ulasmak �c�ndr� shred katman�ndan
            services.AddAccessTokenManagement();//137 IClientAccessTokenCache bunun �c�n ekled�k tan�s�n d�ye paket yuklem�st�k
            services.AddSingleton<PhotoHelper>();//bu helper photostoc m�croservsten fogrof yolunu urls�n� al�yor 146
            services.AddHttpClient<IIdentityService, IdentityService>();//119--services.AddHttpClien �le ekmek sebeb�m�z Identitservice _httpClient dondurgumuz ��nd�r yoksa AddScoped olarak ekleyecekt�k                 


           services.AddScoped<ResourceOwnerPasswordTokenHandler>();//123 ekleme unutuldu-126 ekled�n butun handler serv�s olarak eklen�yr
            services.AddScoped<ClientCredentialTokenHandler>();//136 ekleme unutuldu-126 ekled�n butun handler serv�s olarak eklen�yr

            //154 ServiceExtension yazd�k b�r k�s�m� oraya ald�kk� buras� kalaba�koldu baya
            services.AddHttpClientServices(Configuration);//154 exta�on klasoru ServiceExtension s�n�f�









            ////120 cookie yetk�lend�rme
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, opt =>
            {
                opt.LoginPath = "/Auth/signIn";
                opt.ExpireTimeSpan = TimeSpan.FromDays(60);//60 gun klacak
                opt.SlidingExpiration = true;//60 gun doldukca 60 gun daha uzas�nm� evet uzas�n yap�yoruz
                opt.Cookie.Name = "sincomwebcookiemicroservice";
            });//120 cook� tan�tma










            //services.AddControllersWithViews();//159 bunu gen�slet�ryoruz
            services.AddControllersWithViews().AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CreateCourseInputValidator>());//159
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
                app.UseExceptionHandler("/Home/Error");//150 bu olunca home dek� erroro kod ekl�yoruz
            }
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();//120 eklendi
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
