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
            //191 MassTransit.AspNetCore RabbitMq ayarlarý paketler yukledýn

            services.AddMassTransit(x =>
            {
                x.AddConsumer<CourseNameChangedFotBasketEventConsumer>();//191  evetlarý dýnlemek ýcýn eklýyruz
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(Configuration["RabbitMQUrl"], "/", host =>  //"RabbitMQUrl": "localhost", teký ýsmle ayný ollmalý Configuration["RabbitMQUrl"]
                    {
                        host.Username("guest");//burdaký kullanýcý adý ve þifre  defult gelýyor

                        host.Password("guest");
                    });
                    //-------------------------191
                    cfg.ReceiveEndpoint("course-name-changed-event-basket-service", e =>// burda entpoint tanýmlýyorz ký buraya baglasn
                    {
                        e.ConfigureConsumer<CourseNameChangedFotBasketEventConsumer>(context);//  endPoint teki verýlerý okuyoruz
                    });

                    //-----------------------------------------------------------------------------------

                });

            });
            //5672 kullanýlan default port ayaga kalkýyor,onu takýp etmek ýcýn ýse 15672 portu uzerýnde takpedebýýrz
            services.AddMassTransitHostedService();
            //--------------------------------------------------183 










            //userId
            services.AddHttpContextAccessor();//59 ekledýk ký shredtteký býr sýnýfa aýt metot ýdentýye baglanýp ordaký context uerýnden userýd ulasabýlsýn

            services.AddScoped<ISharedIdentityService, SharedIdentityService>();// refrenas aldýgýmýz býr projenýn sýnýflarýný ve ýnterfacelerýný kaulalnabýlmez normalde bunlar ISharedIdentityService,SharedIdentityService shared projesýndedýr
            //59---------------------------------------------------------

            services.AddScoped<IBasketService, BasketService>();//60 normal proje ýcýndeký ýnterfacelerýný ve sýnýflarýný tanýmlýyoruz



            //giriþ için

            var requireAuthorizePolicy=new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();//62 giriþ yapýmýs user býlgýsý sartý ve token alacak 62
           
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");


            //62 burda authentication tanýmlýyoruz ayrý ktamanda olan proje kendý ayaga kalkýyor ve ordan dagýtýlan authentoýn ýle ayar verýyrouz
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
            {
                opt.Authority = Configuration["IdentityServerURL"];
                opt.Audience = "resourse_basket";//ýdentityserver ýcýndeký confýd dosyasýnda recourse_basket aldým ordan kontrol edecek
                opt.RequireHttpsMetadata = false;
            });//62 basket mýcroservýste kullanýcýlý baglantýlýrdýr



            
           

            //55 settýngs dosyasýný buraya tanýmlýyorus appsettýngs.jpsn RedisSettings tagýndan  okuyacak esletýrecek
            services.Configure<RedisSettings>(Configuration.GetSection("RedisSettings"));//55

            services.AddSingleton<RedisService>(sp =>// 57  redisservise baglanýp onunla býrlýte redissettings ulasýp host ve portlarýný alýyor ve redýs.connet ýle baglanýyor redis verýtabana baglantýsýdýr
            {
                var redisSettings=sp.GetRequiredService<IOptions<RedisSettings>>().Value;
                var redis=new RedisService(redisSettings.Host,redisSettings.Port);
                redis.Connect();
                return redis;
            });//57

           
            
            


            //services.AddControllers();//62 alttaký gýbý genýsletýyoruz  koruma alýtýna alýyoruz yaný gýrýs sartý var
            services.AddControllers(opt =>
            {
                opt.Filters.Add(new AuthorizeFilter(requireAuthorizePolicy));//62 burayý genýsletýyoruz ve butun kontroller  token alamdan bagalanamz,user þartý var  requireAuthorizePolicy bunu yukarda tanýmladýk
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

            app.UseAuthentication();//62 ekledýk giriþ için
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
