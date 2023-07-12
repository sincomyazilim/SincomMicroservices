using FreeCourse.Services.Catalog.Services.Abstract;
using FreeCourse.Services.Catalog.Services.Concrete;
using FreeCourse.Services.Catalog.Settings.Abstract;
using FreeCourse.Services.Catalog.Settings.Concrete;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);


//190 MassTransit.AspNetCore RabbitMq ayarlarý paketler yukledýn

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
builder.Services.AddMassTransitHostedService();
//--------------------------------------------------190








//41 burda authentication tanýmlýyoruz ayrý ktamanda olan proje kendý ayaga kalkýyor ve ordan dagýtýlan authentoýn ýle ayar verýyrouz
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
{
    opt.Authority = builder.Configuration["IdentityServerURL"];
    opt.Audience = "resourse_catalog";//ýdentityserver ýcýndeký confýd dosyasýnda recourse_catalog aldým ordan kontrol edecek
    opt.RequireHttpsMetadata = false;
});//41



builder.Services.AddScoped<ICategoryService, CategoryService>();//24 ekledýk
builder.Services.AddScoped<ICourseService, CourseService>();//25 ekledýk



builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());// 22 automapper ekledýk burayada bunu tanýmladýk
                                                //services.AddControllers();//41 alttaký gýbý genýsletýyoruz
builder.Services.AddControllers(opt =>
{
    opt.Filters.Add(new AuthorizeFilter());//41 burayý genýsletýyoruz ve butun kontroller  token alamdan bagalanamz,user sartý yok
});




//appsetingteký datalarý okuma DatabaseSettings bu sýnýf uzerýnden
builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettings"));//23 eklendý
builder.Services.AddSingleton<IDatabaseSettings>(x =>
{
    return x.GetRequiredService<IOptions<DatabaseSettings>>().Value;
});
//23 bu kod datbasebaglantý kurmak ýcýn kullandýk
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();










var app = builder.Build();

using (var scope=app.Services.CreateScope())
{
    var servicesProvider = scope.ServiceProvider;
    var categoryService = servicesProvider.GetRequiredService<ICategoryService>();
    if (!(await categoryService.GetAllAsync()).Data.Any())
    {
        await   categoryService.CreateCategoryAsync(new FreeCourse.Services.Catalog.Dtos.CategorieDto.CategoryCreateDto { Name = "Asp.net Core Kursu" });
       await categoryService.CreateCategoryAsync(new FreeCourse.Services.Catalog.Dtos.CategorieDto.CategoryCreateDto { Name = "Asp.net Core Api Kursu" });
    }
}







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




























