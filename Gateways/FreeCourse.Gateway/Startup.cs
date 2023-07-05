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
        public void ConfigureServices(IServiceCollection services)// 109 koruma altýna alýyoruz
        {
            //services.AddHttpClient<TokenExhangeDelegateHandler>();//195 hanlerý tanýmlýyoruz



            services.AddAuthentication().AddJwtBearer("GatewayAuthenticationScheme", options =>
            {
               options.Authority = Configuration["IdentityServerURL"];
               options.Audience = "resourse_gateway";
               options.RequireHttpsMetadata = false;
            });



            services.AddOcelot();//105 kutuhanemýzý tanýmladýk
           // services.AddOcelot().AddDelegatingHandler<TokenExhangeDelegateHandler>();195 eklýyoruz ama býzkullanmazyacaz

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            await app.UseOcelot();//bu kutuphane yonlendýrme yapýyor asyn olarak calsýyor async eklendýr 105
        }
    }
}
//burda sadece ýdentýty server ýlae habelesmesýný saglýyoruz ve ýlgýlý ýzýnler tanýmlanýyor kensýý ýcýnde bu  gateway ýcýnde 