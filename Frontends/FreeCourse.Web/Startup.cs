using FreeCourse.Shared.Services.Abstract;
using FreeCourse.Shared.Services.Concrete;
using FreeCourse.Web.Handler;
using FreeCourse.Web.Helpers;
using FreeCourse.Web.Models;
using FreeCourse.Web.Services.Abstract;
using FreeCourse.Web.Services.Concrete;
using IdentityModel.AspNetCore.AccessTokenManagement;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

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
            //119


            services.AddHttpContextAccessor();//119 
            services.AddAccessTokenManagement();//137 IClientAccessTokenCache bunun �c�n ekled�k tan�s�n d�ye paket yuklem�st�k
            services.AddSingleton<PhotoHelper>();//bu helper photostoc m�croservsten fogrof yolunu urls�n� al�yor 146

            services.AddHttpClient<IIdentityService, IdentityService>();//119--services.AddHttpClien �le ekmek sebeb�m�z Identitservice _httpClient dondurgumuz ��nd�r yoksa AddScoped olarak ekleyecekt�k

            services.AddScoped<ISharedIdentityService, SharedIdentityService>();//134 bunu tan�l�yoruz �dent�ty userId almak �c�n            
            
            services .AddScoped<IClientCredentialTokenService, ClientCredentialTokenService>();//135 ClientCredentialTokenService tan�ml�yoruz 
            



            services.AddScoped<ResourceOwnerPasswordTokenHandler>();//123 ekleme unutuldu-126 ekled�n butun handler serv�s olarak eklen�yr
            services.AddScoped<ClientCredentialTokenHandler>();//136 ekleme unutuldu-126 ekled�n butun handler serv�s olarak eklen�yr






            var serviceApiSettings = Configuration.GetSection("ServiceApiSettings").Get<ServiceApiSettings>();//123 


            services.AddHttpClient<IPhotoStockService, PhotoStockService>(opt =>//143 servisler eklen�rken htppcl�ne olarak �stek yapt�klar�nda 
            {//scope olark deg�l  services.AddHttpClient<IPhotoStockService, PhotoStockService> �ekl�nde eklen�r.burdan appsett�n i�ine g�r�yor �c�nde serviceApiSettings bolumu ,boulum �c�nden catalog,ve onunda �c�nden path sec�l�yor

                opt.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseUrl}/{serviceApiSettings.PhotoStock.Path}");
            }).AddHttpMessageHandler<ClientCredentialTokenHandler>();// photostock cagr�ldg�nda token kend�s� yetk�s�n� g�sterecek 





            services.AddHttpClient<ICatalogService, CatalogService>(opt =>//132 servisler eklen�rken htppcl�ne olarak �stek yapt�klar�nda 
            {//scope olark deg�l  services.AddHttpClient<ICatalogService, CatalogService> �ekl�nde eklen�r.burdan appsett�n i�ine g�r�yor �c�nde serviceApiSettings bolumu ,boulum �c�nden catalog,ve onunda �c�nden path sec�l�yor

                opt.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseUrl}/{serviceApiSettings.Catalog.Path}");
            }).AddHttpMessageHandler<ClientCredentialTokenHandler>();// catalog cagr�ldg�nda token kend�s� yetk�s�n� g�sterecek category ve course g�reb�l�rm�y�m g�remzm�y�m 136
      

            

            services.AddHttpClient<IUserService, UserService>(opt =>//123 userService cagr�ld�g�nda baseurl gonder ve token basl�ga ekle gonder
            {
                opt.BaseAddress = new Uri(serviceApiSettings.IdentityBaseUrl);//appsetingtek� ver�ler� okumak �c�n ve �c�ndek� url almak ve UserService cagr�ld�g�nda otomat�k http://localhost:5001/ bu l�nk� ekl�yor bas�na UserService s�n�f�na bak getuser medudana ac�klama yaptm
            }).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();//123 --125 AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>(); ekled�k cunku bu ekled�g�m�z �dent�tyserver �stek sorgu yan� g�t�g�nde basn�a tkne ekleyen s�n�ft�r delagett�r











            ////120 cookie yetk�lend�rme
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, opt =>
            {
                opt.LoginPath = "/Auth/signIn";
                opt.ExpireTimeSpan = TimeSpan.FromDays(60);//60 gun klacak
                opt.SlidingExpiration = true;//60 gun doldukca 60 gun daha uzas�nm� evet uzas�n yap�yoruz
                opt.Cookie.Name = "sincomwebcookiemicroservice";
            });//120 cook� tan�tma









            services.AddControllersWithViews();
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
