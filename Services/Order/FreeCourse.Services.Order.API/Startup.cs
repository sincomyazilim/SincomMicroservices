
using FreeCourse.Services.Order.Application.ConsumersRabbitmq;
using FreeCourse.Services.Order.Application.ConsumersRabbitmq.PublýshEvent;
using FreeCourse.Services.Order.Infrastructure.Context;
using FreeCourse.Shared.Services.Abstract;
using FreeCourse.Shared.Services.Concrete;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;

namespace FreeCourse.Services.Order.API
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
            //187 kuyruk fakepayment projesýndeký rabbýtmq dadýr orda rabýtmq ya baglatý cumlemýz vardý onu buryada tanýmlýyoruz 183 videode bu projeye eklemýstýk


            //183 MassTransit.AspNetCore RabbitMq ayarlarý paketler yukledýn //187 ekledýk bunu gelýstýrecez

            services.AddMassTransit(x =>
            {
                x.AddConsumer<CreateOrderMessageCommandConsumer>();//187 commmand gonderdýk kuyruga 
                x.AddConsumer<CourseNameChangedEventConsumer>();//191  evetlarý dýnlemek ýcýn eklýyruz

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(Configuration["RabbitMQUrl"], "/", host =>  // bu projenýn appsetýnden yolunu okuyaak onun ýcýn appsetýnge ekleyelým urlyý
                    {
                        host.Username("guest");//burdaký kullanýcý adý ve þifre  defult gelýyor

                        host.Password("guest");
                    });
                    //-----------------------------------------------------------------------------------187 ekledýk
                    cfg.ReceiveEndpoint("create-order-service", e =>
                    {
                        e.ConfigureConsumer<CreateOrderMessageCommandConsumer>(context);// kuyrukdtaký create-order-service adýndaký endPoint teki verýlerý okuyoruz
                    });
                    
                    
                    //-------------------------191
                    cfg.ReceiveEndpoint("course-name-changed-event-order-service", e =>// burda entpoint tanýmlýyorz ký buraya baglasn
                    {
                        e.ConfigureConsumer<CourseNameChangedEventConsumer>(context);//  endPoint teki verýlerý okuyoruz
                    });

                    //-----------------------------------------------------------------------------------

                });

            });
            //5672 kullanýlan default port ayaga kalkýyor,onu takýp etmek ýcýn ýse 15672 portu uzerýnde takpedebýýrz
            services.AddMassTransitHostedService();
            //--------------------------------------------------183   




            //-----------------------------187----------------------------------------------------




            //94 sqlserver baglanmak ýcýn kod yazdýk
            services.AddDbContext<OrderDbContext>(opt =>
            {                                                     //DefaultConnection baglntý cumlesýný appsettýng.json dan alýyor.
                opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), configure =>
                {
                    configure.MigrationsAssembly("FreeCourse.Services.Order.Infrastructure");//configure miration baska katmandaký proede oldugu ýcýn ve bunu anlamasýýcýn ona yol gösterýyoruz proje adý FreeCourse.Services.Order.Infrastructure
                });
            });//94
            
            
            //95 application katmaný handler eklenýyor
            services.AddMediatR(typeof(FreeCourse.Services.Order.Application.Handlers.CreateOrderCommandHandler).Assembly);
            services.AddScoped<ISharedIdentityService, SharedIdentityService>();//95 userID Identýyserverdan getýrmek cýn shared katmaný eklenýyor
            services.AddHttpContextAccessor(); //bu katman shrad katmanýndaký UserId tanýmasý ýcýn eklenýyor 
                                               //95-----------------------------------------------------------------------------


            //giriþ için

            var requireAuthorizePolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();//96 giriþ yapýmýs user býlgýsý sartý ve token alacak 96

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");//sub kelýmesý dönuste sýlýyordu kendý maplemeýsnde Id yapýyordu býzde bu kodla donusu sub býrakdýyorzu


            //96 burda authentication tanýmlýyoruz ayrý ktamanda olan proje kendý ayaga kalkýyor ve ordan dagýtýlan authentoýn ýle ayar verýyrouz
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
            {
                opt.Authority = Configuration["IdentityServerURL"];//bu kýsýmda order-appsettýngs.josn lýnk verýlecek.lýnk budur "IdentityServerURL": "http://localhost:5001", //identiserver ie bu mýcroservýs habdar oluyor
                opt.Audience = "resourse_order";//ýdentityserver ýcýndeký confýg dosyasýnda recourse_order aldým ordan kontrol edecek
                opt.RequireHttpsMetadata = false; //https ýstemýyoruz
            });//96 basket mýcroservýste kullanýcýlý baglantýlýrdýr

            //services.AddControllers();//96 alttaký gýbý genýsletýyoruz  koruma alýtýna alýyoruz yaný gýrýs sartý var
            services.AddControllers(opt =>
            {
                opt.Filters.Add(new AuthorizeFilter(requireAuthorizePolicy));//96 burayý genýsletýyoruz ve butun kontroller  token alamdan bagalanamz,user þartý var  requireAuthorizePolicy bunu yukarda tanýmladýk
            });



            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FreeCourse.Services.Order.API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FreeCourse.Services.Order.API v1"));
            }

            app.UseRouting();
            app.UseAuthentication();//96 ekledýk giriþ için
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
