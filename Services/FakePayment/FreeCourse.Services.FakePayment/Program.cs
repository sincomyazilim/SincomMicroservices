using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//183 MassTransit.AspNetCore RabbitMq ayarlarý paketler yukledýn

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMQUrl"], "/", host =>  //"RabbitMQUrl": "localhost", teký ýsmle ayný ollmalý Configuration["RabbitMQUrl"]
        {
            host.Username("guest");//burdaký kullanýcý adý ve þifre  defult gelýyor

            host.Password("guest");
        });

    });

});
//5672 kullanýlan default port ayaga kalkýyor,onu takýp etmek ýcýn ýse 15672 portu uzerýnde takpedebýýrz
//builder.Services.AddMassTransitHostedService();// bu koda 8.1 versýnda gerekyok
//--------------------------------------------------183   




//giriþ için

var requireAuthorizePolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();//101 giriþ yapýmýs user býlgýsý sartý ve token alacak 

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");//sub kelýmesý dönuste sýlýyordu kendý maplemeýsnde Id yapýyordu býzde bu kodla donusu sub býrakdýyorzu


//96 burda authentication tanýmlýyoruz ayrý ktamanda olan proje kendý ayaga kalkýyor ve ordan dagýtýlan authentoýn ýle ayar verýyrouz
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
{
    opt.Authority = builder.Configuration["IdentityServerURL"];//bu kýsýmda order-appsettýngs.josn lýnk verýlecek.lýnk budur "IdentityServerURL": "http://localhost:5001", //identiserver ie bu mýcroservýs habdar oluyor
    opt.Audience = "resourse_fakepayment";//ýdentityserver ýcýndeký confýg dosyasýnda recourse_order aldým ordan kontrol edecek
    opt.RequireHttpsMetadata = false; //https ýstemýyoruz
});//96 basket mýcroservýste kullanýcýlý baglantýlýrdýr

//services.AddControllers();//101 alttaký gýbý genýsletýyoruz  koruma alýtýna alýyoruz yaný gýrýs sartý var
builder.Services.AddControllers(opt =>
{
    opt.Filters.Add(new AuthorizeFilter(requireAuthorizePolicy));//101 burayý genýsletýyoruz ve butun kontroller  token alamdan bagalanamz,user þartý var  requireAuthorizePolicy bunu yukarda tanýmladýk
});






var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
app.UseAuthentication();//101
app.UseAuthorization();

app.MapControllers();

app.Run();
