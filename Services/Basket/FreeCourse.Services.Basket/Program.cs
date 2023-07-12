

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
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);





builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<CourseNameChangedFotBasketEventConsumer>();//191  evetlarý dýnlemek ýcýn eklýyruz
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMQUrl"], "/", host =>  //"RabbitMQUrl": "localhost", teký ýsmle ayný ollmalý Configuration["RabbitMQUrl"]
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
//builder.Services.AddMassTransitHostedService();//8 versýyonda ýptal edýyorzu
//--------------------------------------------------183 


//userId
builder.Services.AddHttpContextAccessor();//59 ekledýk ký shredtteký býr sýnýfa aýt metot ýdentýye baglanýp ordaký context uerýnden userýd ulasabýlsýn

builder.Services.AddScoped<ISharedIdentityService, SharedIdentityService>();// refrenas aldýgýmýz býr projenýn sýnýflarýný ve ýnterfacelerýný kaulalnabýlmez normalde bunlar ISharedIdentityService,SharedIdentityService shared projesýndedýr
                                                                            //59---------------------------------------------------------

builder.Services.AddScoped<IBasketService, BasketService>();//60 normal proje ýcýndeký ýnterfacelerýný ve sýnýflarýný tanýmlýyoruz



//giriþ için

var requireAuthorizePolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();//62 giriþ yapýmýs user býlgýsý sartý ve token alacak 62

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");


//62 burda authentication tanýmlýyoruz ayrý ktamanda olan proje kendý ayaga kalkýyor ve ordan dagýtýlan authentoýn ýle ayar verýyrouz
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
{
    opt.Authority = builder.Configuration["IdentityServerURL"];
    opt.Audience = "resourse_basket";//ýdentityserver ýcýndeký confýd dosyasýnda recourse_basket aldým ordan kontrol edecek
    opt.RequireHttpsMetadata = false;
});//62 basket mýcroservýste kullanýcýlý baglantýlýrdýr






//55 settýngs dosyasýný buraya tanýmlýyorus appsettýngs.jpsn RedisSettings tagýndan  okuyacak esletýrecek
    builder.Services.Configure<RedisSettings>(builder.Configuration.GetSection("RedisSettings"));//55

builder.Services.AddSingleton<RedisService>(sp =>// 57  redisservise baglanýp onunla býrlýte redissettings ulasýp host ve portlarýný alýyor ve redýs.connet ýle baglanýyor redis verýtabana baglantýsýdýr
{
    var redisSettings = sp.GetRequiredService<IOptions<RedisSettings>>().Value;
    var redis = new RedisService(redisSettings.Host, redisSettings.Port);
    redis.Connect();
    return redis;
});//57







builder.Services.AddControllers(opt =>
{
    opt.Filters.Add(new AuthorizeFilter(requireAuthorizePolicy));//62 burayý genýsletýyoruz ve butun kontroller  token alamdan bagalanamz,user þartý var  requireAuthorizePolicy bunu yukarda tanýmladýk
});









var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();































































