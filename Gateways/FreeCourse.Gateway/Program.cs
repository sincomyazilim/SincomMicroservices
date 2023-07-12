using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Values;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile($"configuration.{builder.Environment.EnvironmentName.ToLower()}.json").AddEnvironmentVariables();
//bu mettot ýle configuration.development ve configuration.production ulasabýlýyoruz 106

//services.AddHttpClient<TokenExhangeDelegateHandler>();//195 hanlerý tanýmlýyoruz



builder.Services.AddAuthentication().AddJwtBearer("GatewayAuthenticationScheme", options =>
{
    options.Authority =builder.Configuration["IdentityServerURL"];
    options.Audience = "resourse_gateway";
    options.RequireHttpsMetadata = false;
});



builder.Services.AddOcelot();//105 kutuhanemýzý tanýmladýk
                     // services.AddOcelot().AddDelegatingHandler<TokenExhangeDelegateHandler>();195 eklýyoruz ama býzkullanmazyacaz



var app = builder.Build();


await app.UseOcelot();

app.UseAuthorization();
app.UseDeveloperExceptionPage();
app.MapControllers();

app.Run();







































