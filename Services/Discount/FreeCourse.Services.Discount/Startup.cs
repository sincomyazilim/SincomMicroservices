using FreeCourse.Services.Discount.Services.Abstract;
using FreeCourse.Services.Discount.Services.Conrete;
using FreeCourse.Shared.Services.Abstract;
using FreeCourse.Shared.Services.Concrete;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCourse.Services.Discount
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
            //userId �c�n
            services.AddHttpContextAccessor();//74 ekled�k k� shredttek� b�r s�n�fa a�t metot �dent�ye baglan�p ordak� context uer�nden user�d ulasab�ls�n

            services.AddScoped<ISharedIdentityService, SharedIdentityService>();// refrenas ald�g�m�z b�r projen�n s�n�flar�n� ve �nterfaceler�n� kaulalnab�lmez normalde bunlar ISharedIdentityService,SharedIdentityService shared projes�nded�r
            //74--------------------------------------------------------


            services.AddScoped<IDiscountService, DiscountService>();//75 normal proje �c�ndek� �nterfaceler�n� ve s�n�flar�n� tan�ml�yoruz



            //giri� i�in

            var requireAuthorizePolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();//74 giri� yap�m�s user b�lg�s� sart� ve token alacak 74

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");

            //74 burda authentication tan�ml�yoruz ayr� ktamanda olan proje kend� ayaga kalk�yor ve ordan dag�t�lan authento�n �le ayar ver�yrouz
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
            {
                opt.Authority = Configuration["IdentityServerURL"];
                opt.Audience = "resourse_discount";//�dentityserver �c�ndek� conf�d dosyas�nda recourse_basket ald�m ordan kontrol edecek
                opt.RequireHttpsMetadata = false;
            });//74 basket m�croserv�ste kullan�c�l� baglant�l�rd�r

            //services.AddControllers();//74 alttak� g�b� gen�slet�yoruz  koruma al�t�na al�yoruz yan� g�r�s sart� var
            services.AddControllers(opt =>
            {
                opt.Filters.Add(new AuthorizeFilter(requireAuthorizePolicy));//74 buray� gen�slet�yoruz ve butun kontroller  token alamdan bagalanamz,user �art� var  requireAuthorizePolicy bunu yukarda tan�mlad�k
            });




          





            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FreeCourse.Services.Discount", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FreeCourse.Services.Discount v1"));
            }

            app.UseRouting();
            app.UseAuthentication();//74 g�r�s �c�n k�ml�kdogrulama

            app.UseAuthorization();//yetk� k�sm�d�r


           

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
