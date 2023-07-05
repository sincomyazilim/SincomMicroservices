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
            //190 MassTransit.AspNetCore RabbitMq ayarlar� paketler yukled�n

            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(Configuration["RabbitMQUrl"], "/", host =>  //"RabbitMQUrl": "localhost", tek� �smle ayn� ollmal� Configuration["RabbitMQUrl"]
                    {
                        host.Username("guest");//burdak� kullan�c� ad� ve �ifre  defult gel�yor

                        host.Password("guest");
                    });

                });

            });
            //5672 kullan�lan default port ayaga kalk�yor,onu tak�p etmek �c�n �se 15672 portu uzer�nde takpedeb��rz
            services.AddMassTransitHostedService();
            //--------------------------------------------------190








            //41 burda authentication tan�ml�yoruz ayr� ktamanda olan proje kend� ayaga kalk�yor ve ordan dag�t�lan authento�n �le ayar ver�yrouz
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
            {
                opt.Authority = Configuration["IdentityServerURL"];
                opt.Audience = "resourse_catalog";//�dentityserver �c�ndek� conf�d dosyas�nda recourse_catalog ald�m ordan kontrol edecek
                opt.RequireHttpsMetadata = false;
            });//41

            

            services.AddScoped<ICategoryService,CategoryService>();//24 ekled�k
            services.AddScoped<ICourseService,CourseService>();//25 ekled�k



            services.AddAutoMapper(typeof(Startup));// 22 automapper ekled�k burayada bunu tan�mlad�k
            //services.AddControllers();//41 alttak� g�b� gen�slet�yoruz
            services.AddControllers(opt =>
            {
                opt.Filters.Add(new AuthorizeFilter());//41 buray� gen�slet�yoruz ve butun kontroller  token alamdan bagalanamz,user sart� yok
            });




            //appsetingtek� datalar� okuma DatabaseSettings bu s�n�f uzer�nden
            services.Configure<DatabaseSettings>(Configuration.GetSection("DatabaseSettings"));//23 eklend�
            services.AddSingleton<IDatabaseSettings>(x => 
            { 
               return x.GetRequiredService<IOptions<DatabaseSettings>>().Value; 
            });
            //23 bu kod datbasebaglant� kurmak �c�n kulland�k

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
            app.UseAuthentication();//41 ekled�k
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
