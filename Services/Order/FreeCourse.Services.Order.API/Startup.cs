
using FreeCourse.Services.Order.Infrastructure.Context;
using FreeCourse.Shared.Services.Abstract;
using FreeCourse.Shared.Services.Concrete;
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
        {//94 sqlserver baglanmak �c�n kod yazd�k
            services.AddDbContext<OrderDbContext>(opt =>
            {                                                     //DefaultConnection baglnt� cumles�n� appsett�ng.json dan al�yor.
                opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), configure =>
                {
                    configure.MigrationsAssembly("FreeCourse.Services.Order.Infrastructure");//configure miration baska katmandak� proede oldugu �c�n ve bunu anlamas��c�n ona yol g�ster�yoruz proje ad� FreeCourse.Services.Order.Infrastructure
                });
            });//94
            
            
            //95 application katman� handler eklen�yor
            services.AddMediatR(typeof(FreeCourse.Services.Order.Application.Handlers.CreateOrderCommandHandler).Assembly);
            services.AddScoped<ISharedIdentityService, SharedIdentityService>();//95 userID Ident�yserverdan get�rmek c�n shared katman� eklen�yor
            services.AddHttpContextAccessor(); //bu katman shrad katman�ndak� UserId tan�mas� �c�n eklen�yor 
                                               //95-----------------------------------------------------------------------------


            //giri� i�in

            var requireAuthorizePolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();//96 giri� yap�m�s user b�lg�s� sart� ve token alacak 96

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");//sub kel�mes� d�nuste s�l�yordu kend� mapleme�snde Id yap�yordu b�zde bu kodla donusu sub b�rakd�yorzu


            //96 burda authentication tan�ml�yoruz ayr� ktamanda olan proje kend� ayaga kalk�yor ve ordan dag�t�lan authento�n �le ayar ver�yrouz
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
            {
                opt.Authority = Configuration["IdentityServerURL"];//bu k�s�mda order-appsett�ngs.josn l�nk ver�lecek.l�nk budur "IdentityServerURL": "http://localhost:5001", //identiserver ie bu m�croserv�s habdar oluyor
                opt.Audience = "resourse_order";//�dentityserver �c�ndek� conf�g dosyas�nda recourse_order ald�m ordan kontrol edecek
                opt.RequireHttpsMetadata = false; //https �stem�yoruz
            });//96 basket m�croserv�ste kullan�c�l� baglant�l�rd�r

            //services.AddControllers();//96 alttak� g�b� gen�slet�yoruz  koruma al�t�na al�yoruz yan� g�r�s sart� var
            services.AddControllers(opt =>
            {
                opt.Filters.Add(new AuthorizeFilter(requireAuthorizePolicy));//96 buray� gen�slet�yoruz ve butun kontroller  token alamdan bagalanamz,user �art� var  requireAuthorizePolicy bunu yukarda tan�mlad�k
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
            app.UseAuthentication();//96 ekled�k giri� i�in
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
