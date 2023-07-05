using FreeCourse.Gateway.DelegateHandlers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCourse.Gateway
{
    public class Startup
    {
        private readonly IConfiguration Configuration;//109

        public Startup(IConfiguration configuration)//109
        {
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)// 109 koruma alt�na al�yoruz
        {
            //services.AddHttpClient<TokenExhangeDelegateHandler>();//195 hanler� tan�ml�yoruz



            services.AddAuthentication().AddJwtBearer("GatewayAuthenticationScheme", options =>
            {
               options.Authority = Configuration["IdentityServerURL"];
               options.Audience = "resourse_gateway";
               options.RequireHttpsMetadata = false;
            });



            services.AddOcelot();//105 kutuhanem�z� tan�mlad�k
           // services.AddOcelot().AddDelegatingHandler<TokenExhangeDelegateHandler>();195 ekl�yoruz ama b�zkullanmazyacaz

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            await app.UseOcelot();//bu kutuphane yonlend�rme yap�yor asyn olarak cals�yor async eklend�r 105
        }
    }
}
//burda sadece �dent�ty server �lae habelesmes�n� sagl�yoruz ve �lg�l� �z�nler tan�mlan�yor kens�� �c�nde bu  gateway �c�nde 