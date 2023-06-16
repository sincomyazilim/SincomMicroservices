using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;

namespace FreeCourse.Services.FakePayment
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
            //giriþ için

            var requireAuthorizePolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();//101 giriþ yapýmýs user býlgýsý sartý ve token alacak 

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");//sub kelýmesý dönuste sýlýyordu kendý maplemeýsnde Id yapýyordu býzde bu kodla donusu sub býrakdýyorzu


            //96 burda authentication tanýmlýyoruz ayrý ktamanda olan proje kendý ayaga kalkýyor ve ordan dagýtýlan authentoýn ýle ayar verýyrouz
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
            {
                opt.Authority = Configuration["IdentityServerURL"];//bu kýsýmda order-appsettýngs.josn lýnk verýlecek.lýnk budur "IdentityServerURL": "http://localhost:5001", //identiserver ie bu mýcroservýs habdar oluyor
                opt.Audience = "resourse_fakepayment";//ýdentityserver ýcýndeký confýg dosyasýnda recourse_order aldým ordan kontrol edecek
                opt.RequireHttpsMetadata = false; //https ýstemýyoruz
            });//96 basket mýcroservýste kullanýcýlý baglantýlýrdýr

            //services.AddControllers();//101 alttaký gýbý genýsletýyoruz  koruma alýtýna alýyoruz yaný gýrýs sartý var
            services.AddControllers(opt =>
            {
                opt.Filters.Add(new AuthorizeFilter(requireAuthorizePolicy));//101 burayý genýsletýyoruz ve butun kontroller  token alamdan bagalanamz,user þartý var  requireAuthorizePolicy bunu yukarda tanýmladýk
            });





            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FreeCourse.Services.FakePayment", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FreeCourse.Services.FakePayment v1"));
            }

            app.UseRouting();
            app.UseAuthentication();//101
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
