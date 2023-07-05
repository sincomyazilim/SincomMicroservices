using FreeCourse.Services.Catalog.Services.Abstract;
using FreeCourse.Services.Catalog.Services.Concrete;
using FreeCourse.Services.Catalog.Settings.Abstract;
using FreeCourse.Services.Catalog.Settings.Concrete;
using FreeCourse.Shared.Services.Abstract;
using FreeCourse.Shared.Services.Concrete;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCourse.Services.Catalog
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
            //190 MassTransit.AspNetCore RabbitMq ayarlarý paketler yukledýn

            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(Configuration["RabbitMQUrl"], "/", host =>  //"RabbitMQUrl": "localhost", teký ýsmle ayný ollmalý Configuration["RabbitMQUrl"]
                    {
                        host.Username("guest");//burdaký kullanýcý adý ve þifre  defult gelýyor

                        host.Password("guest");
                    });

                });

            });
            //5672 kullanýlan default port ayaga kalkýyor,onu takýp etmek ýcýn ýse 15672 portu uzerýnde takpedebýýrz
            services.AddMassTransitHostedService();
            //--------------------------------------------------190








            //41 burda authentication tanýmlýyoruz ayrý ktamanda olan proje kendý ayaga kalkýyor ve ordan dagýtýlan authentoýn ýle ayar verýyrouz
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
            {
                opt.Authority = Configuration["IdentityServerURL"];
                opt.Audience = "resourse_catalog";//ýdentityserver ýcýndeký confýd dosyasýnda recourse_catalog aldým ordan kontrol edecek
                opt.RequireHttpsMetadata = false;
            });//41

            

            services.AddScoped<ICategoryService,CategoryService>();//24 ekledýk
            services.AddScoped<ICourseService,CourseService>();//25 ekledýk



            services.AddAutoMapper(typeof(Startup));// 22 automapper ekledýk burayada bunu tanýmladýk
            //services.AddControllers();//41 alttaký gýbý genýsletýyoruz
            services.AddControllers(opt =>
            {
                opt.Filters.Add(new AuthorizeFilter());//41 burayý genýsletýyoruz ve butun kontroller  token alamdan bagalanamz,user sartý yok
            });




            //appsetingteký datalarý okuma DatabaseSettings bu sýnýf uzerýnden
            services.Configure<DatabaseSettings>(Configuration.GetSection("DatabaseSettings"));//23 eklendý
            services.AddSingleton<IDatabaseSettings>(x => 
            { 
               return x.GetRequiredService<IOptions<DatabaseSettings>>().Value; 
            });
            //23 bu kod datbasebaglantý kurmak ýcýn kullandýk

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FreeCourse.Services.Catalog", Version = "v1" });
            });
         
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FreeCourse.Services.Catalog v1"));
            }

           



            app.UseRouting();
            app.UseAuthentication();//41 ekledýk
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
