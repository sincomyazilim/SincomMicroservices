using FreeCourse.Web.Handler;
using FreeCourse.Web.Helpers;
using FreeCourse.Web.Models;
using FreeCourse.Web.Services.Abstract;
using FreeCourse.Web.Services.Concrete;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Configuration;
using System;
using FreeCourse.Web.Extensions;
using FluentValidation.AspNetCore;
using FreeCourse.Web.Validator;

var builder = WebApplication.CreateBuilder(args);


builder.Services.Configure<ServiceApiSettings>(builder.Configuration.GetSection("ServiceApiSettings"));//115 burda appsettýng dosyasýndan verý okuyoruz ServiceApiSettings bu tag altýndakýnden. oda model klasorunde ServiceApiSettings sýnýfýdn 
builder.Services.Configure<ClientSettings>(builder.Configuration.GetSection("ClientSettings"));//116 burda appsettýng dosyasýndan verý okuyoruz ClientSettings bu tag altýndakýnden. oda model klasorunde ClientSettings sýnýfýdn 

builder.Services.AddHttpContextAccessor();//119 UserId ulasmak ýcýndrý shred katmanýndan
builder.Services.AddAccessTokenManagement();//137 IClientAccessTokenCache bunun ýcýn ekledýk tanýsýn dýye paket yuklemýstýk
builder.Services.AddSingleton<PhotoHelper>();//bu helper photostoc mýcroservsten fogrof yolunu urlsýný alýyor 146
builder.Services.AddHttpClient<IIdentityService, IdentityService>();//119--services.AddHttpClien ýle ekmek sebebýmýz Identitservice _httpClient dondurgumuz ýýndýr yoksa AddScoped olarak ekleyecektýk                 


builder.Services.AddScoped<ResourceOwnerPasswordTokenHandler>();//123 ekleme unutuldu-126 ekledýn butun handler servýs olarak eklenýyr
builder.Services.AddScoped<ClientCredentialTokenHandler>();//136 ekleme unutuldu-126 ekledýn butun handler servýs olarak eklenýyr

//154 ServiceExtension yazdýk býr kýsýmý oraya aldýkký burasý kalabaýkoldu baya
builder.Services.AddHttpClientServices(builder.Configuration);//154 extaþon klasoru ServiceExtension sýnýfý









////120 cookie yetkýlendýrme
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, opt =>
{
    opt.LoginPath = "/Auth/signIn";
    opt.ExpireTimeSpan = TimeSpan.FromDays(60);//60 gun klacak
    opt.SlidingExpiration = true;//60 gun doldukca 60 gun daha uzasýnmý evet uzasýn yapýyoruz
    opt.Cookie.Name = "sincomwebcookiemicroservice";
});//120 cooký tanýtma




builder.Services.AddFluentValidationAutoValidation();

//services.AddControllersWithViews();//159 bunu genýsletýryoruz
builder.Services.AddControllersWithViews().AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CreateCourseInputValidator>());//159







var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseDeveloperExceptionPage();
}
app.UseStaticFiles();
app.UseAuthentication();//120 eklendi
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
