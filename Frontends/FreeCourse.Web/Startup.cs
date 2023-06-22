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
            services.Configure<ServiceApiSettings>(Configuration.GetSection("ServiceApiSettings"));//115 burda appsettýng dosyasýndan verý okuyoruz ServiceApiSettings bu tag altýndakýnden. oda model klasorunde ServiceApiSettings sýnýfýdn 

            services.Configure<ClientSettings>(Configuration.GetSection("ClientSettings"));//116 burda appsettýng dosyasýndan verý okuyoruz ClientSettings bu tag altýndakýnden. oda model klasorunde ClientSettings sýnýfýdn 
            //119


            services.AddHttpContextAccessor();//119 
            services.AddAccessTokenManagement();//137 IClientAccessTokenCache bunun ýcýn ekledýk tanýsýn dýye paket yuklemýstýk
            services.AddSingleton<PhotoHelper>();//bu helper photostoc mýcroservsten fogrof yolunu urlsýný alýyor 146

            services.AddHttpClient<IIdentityService, IdentityService>();//119--services.AddHttpClien ýle ekmek sebebýmýz Identitservice _httpClient dondurgumuz ýýndýr yoksa AddScoped olarak ekleyecektýk

            services.AddScoped<ISharedIdentityService, SharedIdentityService>();//134 bunu tanýlýyoruz ýdentýty userId almak ýcýn            
            
            services .AddScoped<IClientCredentialTokenService, ClientCredentialTokenService>();//135 ClientCredentialTokenService tanýmlýyoruz 
            



            services.AddScoped<ResourceOwnerPasswordTokenHandler>();//123 ekleme unutuldu-126 ekledýn butun handler servýs olarak eklenýyr
            services.AddScoped<ClientCredentialTokenHandler>();//136 ekleme unutuldu-126 ekledýn butun handler servýs olarak eklenýyr






            var serviceApiSettings = Configuration.GetSection("ServiceApiSettings").Get<ServiceApiSettings>();//123 


            services.AddHttpClient<IPhotoStockService, PhotoStockService>(opt =>//143 servisler eklenýrken htppclýne olarak ýstek yaptýklarýnda 
            {//scope olark degýl  services.AddHttpClient<IPhotoStockService, PhotoStockService> þeklýnde eklenýr.burdan appsettýn içine gýrýyor ýcýnde serviceApiSettings bolumu ,boulum ýcýnden catalog,ve onunda ýcýnden path secýlýyor

                opt.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseUrl}/{serviceApiSettings.PhotoStock.Path}");
            }).AddHttpMessageHandler<ClientCredentialTokenHandler>();// photostock cagrýldgýnda token kendýsý yetkýsýný gösterecek 





            services.AddHttpClient<ICatalogService, CatalogService>(opt =>//132 servisler eklenýrken htppclýne olarak ýstek yaptýklarýnda 
            {//scope olark degýl  services.AddHttpClient<ICatalogService, CatalogService> þeklýnde eklenýr.burdan appsettýn içine gýrýyor ýcýnde serviceApiSettings bolumu ,boulum ýcýnden catalog,ve onunda ýcýnden path secýlýyor

                opt.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseUrl}/{serviceApiSettings.Catalog.Path}");
            }).AddHttpMessageHandler<ClientCredentialTokenHandler>();// catalog cagrýldgýnda token kendýsý yetkýsýný gösterecek category ve course görebýlýrmýyým göremzmýyým 136
      

            

            services.AddHttpClient<IUserService, UserService>(opt =>//123 userService cagrýldýgýnda baseurl gonder ve token baslýga ekle gonder
            {
                opt.BaseAddress = new Uri(serviceApiSettings.IdentityBaseUrl);//appsetingteký verýlerý okumak ýcýn ve ýcýndeký url almak ve UserService cagrýldýgýnda otomatýk http://localhost:5001/ bu lýnký eklýyor basýna UserService sýnýfýna bak getuser medudana acýklama yaptm
            }).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();//123 --125 AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>(); ekledýk cunku bu ekledýgýmýz ýdentýtyserver ýstek sorgu yaný gýtýgýnde basnýa tkne ekleyen sýnýftýr delagettýr











            ////120 cookie yetkýlendýrme
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, opt =>
            {
                opt.LoginPath = "/Auth/signIn";
                opt.ExpireTimeSpan = TimeSpan.FromDays(60);//60 gun klacak
                opt.SlidingExpiration = true;//60 gun doldukca 60 gun daha uzasýnmý evet uzasýn yapýyoruz
                opt.Cookie.Name = "sincomwebcookiemicroservice";
            });//120 cooký tanýtma









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
                app.UseExceptionHandler("/Home/Error");//150 bu olunca home deký erroro kod eklýyoruz
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
