using FreeCourse.Services.Basket.ConsumersRabbitmq.PublisherEvet;
using FreeCourse.Services.Basket.Services.Abstract;
using FreeCourse.Services.Basket.Services.Concrete;
using FreeCourse.Services.Basket.Settings;
using FreeCourse.Shared.Services.Abstract;
using FreeCourse.Shared.Services.Concrete;
using MassTransit;
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
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCourse.Services.Basket
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
            //191 MassTransit.AspNetCore RabbitMq ayarlar� paketler yukled�n

            services.AddMassTransit(x =>
            {
                x.AddConsumer<CourseNameChangedFotBasketEventConsumer>();//191  evetlar� d�nlemek �c�n ekl�yruz
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(Configuration["RabbitMQUrl"], "/", host =>  //"RabbitMQUrl": "localhost", tek� �smle ayn� ollmal� Configuration["RabbitMQUrl"]
                    {
                        host.Username("guest");//burdak� kullan�c� ad� ve �ifre  defult gel�yor

                        host.Password("guest");
                    });
                    //-------------------------191
                    cfg.ReceiveEndpoint("course-name-changed-event-basket-service", e =>// burda entpoint tan�ml�yorz k� buraya baglasn
                    {
                        e.ConfigureConsumer<CourseNameChangedFotBasketEventConsumer>(context);//  endPoint teki ver�ler� okuyoruz
                    });

                    //-----------------------------------------------------------------------------------

                });

            });
            //5672 kullan�lan default port ayaga kalk�yor,onu tak�p etmek �c�n �se 15672 portu uzer�nde takpedeb��rz
            services.AddMassTransitHostedService();
            //--------------------------------------------------183 










            //userId
            services.AddHttpContextAccessor();//59 ekled�k k� shredttek� b�r s�n�fa a�t metot �dent�ye baglan�p ordak� context uer�nden user�d ulasab�ls�n

            services.AddScoped<ISharedIdentityService, SharedIdentityService>();// refrenas ald�g�m�z b�r projen�n s�n�flar�n� ve �nterfaceler�n� kaulalnab�lmez normalde bunlar ISharedIdentityService,SharedIdentityService shared projes�nded�r
            //59---------------------------------------------------------

            services.AddScoped<IBasketService, BasketService>();//60 normal proje �c�ndek� �nterfaceler�n� ve s�n�flar�n� tan�ml�yoruz



            //giri� i�in

            var requireAuthorizePolicy=new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();//62 giri� yap�m�s user b�lg�s� sart� ve token alacak 62
           
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");


            //62 burda authentication tan�ml�yoruz ayr� ktamanda olan proje kend� ayaga kalk�yor ve ordan dag�t�lan authento�n �le ayar ver�yrouz
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
            {
                opt.Authority = Configuration["IdentityServerURL"];
                opt.Audience = "resourse_basket";//�dentityserver �c�ndek� conf�d dosyas�nda recourse_basket ald�m ordan kontrol edecek
                opt.RequireHttpsMetadata = false;
            });//62 basket m�croserv�ste kullan�c�l� baglant�l�rd�r



            
           

            //55 sett�ngs dosyas�n� buraya tan�ml�yorus appsett�ngs.jpsn RedisSettings tag�ndan  okuyacak eslet�recek
            services.Configure<RedisSettings>(Configuration.GetSection("RedisSettings"));//55

            services.AddSingleton<RedisService>(sp =>// 57  redisservise baglan�p onunla b�rl�te redissettings ulas�p host ve portlar�n� al�yor ve red�s.connet �le baglan�yor redis ver�tabana baglant�s�d�r
            {
                var redisSettings=sp.GetRequiredService<IOptions<RedisSettings>>().Value;
                var redis=new RedisService(redisSettings.Host,redisSettings.Port);
                redis.Connect();
                return redis;
            });//57

           
            
            


            //services.AddControllers();//62 alttak� g�b� gen�slet�yoruz  koruma al�t�na al�yoruz yan� g�r�s sart� var
            services.AddControllers(opt =>
            {
                opt.Filters.Add(new AuthorizeFilter(requireAuthorizePolicy));//62 buray� gen�slet�yoruz ve butun kontroller  token alamdan bagalanamz,user �art� var  requireAuthorizePolicy bunu yukarda tan�mlad�k
            });


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FreeCourse.Services.Basket", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FreeCourse.Services.Basket v1"));
            }

            app.UseRouting();

            app.UseAuthentication();//62 ekled�k giri� i�in
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
