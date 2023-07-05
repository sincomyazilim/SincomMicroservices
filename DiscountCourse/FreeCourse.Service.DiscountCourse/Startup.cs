using FreeCourse.Service.DiscountCourse.Services.Abstract;
using FreeCourse.Service.DiscountCourse.Services.Concrete;
using FreeCourse.Shared.Services.Abstract;
using FreeCourse.Shared.Services.Concrete;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCourse.Service.DiscountCourse
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();


            //userId ýcýn
            services.AddHttpContextAccessor();//74 ekledýk ký shredtteký býr sýnýfa aýt metot ýdentýye baglanýp ordaký context uerýnden userýd ulasabýlsýn

            services.AddScoped<ISharedIdentityService, SharedIdentityService>();// refrenas aldýgýmýz býr projenýn sýnýflarýný ve ýnterfacelerýný kaulalnabýlmez normalde bunlar ISharedIdentityService,SharedIdentityService shared projesýndedýr
            //74--------------------------------------------------------


            services.AddScoped<IDiscountCourseService, DiscountCourseService>();//75 normal proje ýcýndeký ýnterfacelerýný ve sýnýflarýný tanýmlýyoruz



            //giriþ için

            var requireAuthorizePolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();//74 giriþ yapýmýs user býlgýsý sartý ve token alacak 74

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");

            //74 burda authentication tanýmlýyoruz ayrý ktamanda olan proje kendý ayaga kalkýyor ve ordan dagýtýlan authentoýn ýle ayar verýyrouz
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
            {
                opt.Authority = Configuration["IdentityServerURL"];
                opt.Audience = "resourse_discountcourse";//ýdentityserver ýcýndeký confýd dosyasýnda recourse_basket aldým ordan kontrol edecek
                opt.RequireHttpsMetadata = false;
            });//74 basket mýcroservýste kullanýcýlý baglantýlýrdýr

            //services.AddControllers();//74 alttaký gýbý genýsletýyoruz  koruma alýtýna alýyoruz yaný gýrýs sartý var
            services.AddControllers(opt =>
            {
                opt.Filters.Add(new AuthorizeFilter(requireAuthorizePolicy));//74 burayý genýsletýyoruz ve butun kontroller  token alamdan bagalanamz,user þartý var  requireAuthorizePolicy bunu yukarda tanýmladýk
            });
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();//74 gýrýs ýcýn kýmlýkdogrulama
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
