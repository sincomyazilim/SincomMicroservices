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
            services.Configure<ServiceApiSettings>(Configuration.GetSection("ServiceApiSettings"));//115 burda appsettýng dosyasýndan verý okuyoruz ServiceApiSettings bu tag altýndakýnden. oda model klasorunde ServiceApiSettings sýnýfýdn 
            services.Configure<ClientSettings>(Configuration.GetSection("ClientSettings"));//116 burda appsettýng dosyasýndan verý okuyoruz ClientSettings bu tag altýndakýnden. oda model klasorunde ClientSettings sýnýfýdn 
         
            services.AddHttpContextAccessor();//119 UserId ulasmak ýcýndrý shred katmanýndan
            services.AddAccessTokenManagement();//137 IClientAccessTokenCache bunun ýcýn ekledýk tanýsýn dýye paket yuklemýstýk
            services.AddSingleton<PhotoHelper>();//bu helper photostoc mýcroservsten fogrof yolunu urlsýný alýyor 146
            services.AddHttpClient<IIdentityService, IdentityService>();//119--services.AddHttpClien ýle ekmek sebebýmýz Identitservice _httpClient dondurgumuz ýýndýr yoksa AddScoped olarak ekleyecektýk                 


           services.AddScoped<ResourceOwnerPasswordTokenHandler>();//123 ekleme unutuldu-126 ekledýn butun handler servýs olarak eklenýyr
            services.AddScoped<ClientCredentialTokenHandler>();//136 ekleme unutuldu-126 ekledýn butun handler servýs olarak eklenýyr

            //154 ServiceExtension yazdýk býr kýsýmý oraya aldýkký burasý kalabaýkoldu baya
            services.AddHttpClientServices(Configuration);//154 extaþon klasoru ServiceExtension sýnýfý









            ////120 cookie yetkýlendýrme
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, opt =>
            {
                opt.LoginPath = "/Auth/signIn";
                opt.ExpireTimeSpan = TimeSpan.FromDays(60);//60 gun klacak
                opt.SlidingExpiration = true;//60 gun doldukca 60 gun daha uzasýnmý evet uzasýn yapýyoruz
                opt.Cookie.Name = "sincomwebcookiemicroservice";
            });//120 cooký tanýtma










            //services.AddControllersWithViews();//159 bunu genýsletýryoruz
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
