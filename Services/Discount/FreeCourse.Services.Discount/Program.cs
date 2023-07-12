using FreeCourse.Services.Discount.Services.Abstract;
using FreeCourse.Services.Discount.Services.Conrete;
using FreeCourse.Shared.Services.Abstract;
using FreeCourse.Shared.Services.Concrete;
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

//userId ýcýn
builder.Services.AddHttpContextAccessor();//74 ekledýk ký shredtteký býr sýnýfa aýt metot ýdentýye baglanýp ordaký context uerýnden userýd ulasabýlsýn

builder.Services.AddScoped<ISharedIdentityService, SharedIdentityService>();// refrenas aldýgýmýz býr projenýn sýnýflarýný ve ýnterfacelerýný kaulalnabýlmez normalde bunlar ISharedIdentityService,SharedIdentityService shared projesýndedýr
                                                                    //74--------------------------------------------------------


builder.Services.AddScoped<IDiscountService, DiscountService>();//75 normal proje ýcýndeký ýnterfacelerýný ve sýnýflarýný tanýmlýyoruz



//giriþ için

var requireAuthorizePolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();//74 giriþ yapýmýs user býlgýsý sartý ve token alacak 74

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");

//74 burda authentication tanýmlýyoruz ayrý ktamanda olan proje kendý ayaga kalkýyor ve ordan dagýtýlan authentoýn ýle ayar verýyrouz
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
{
    opt.Authority = builder.Configuration["IdentityServerURL"];
    opt.Audience = "resourse_discount";//ýdentityserver ýcýndeký confýd dosyasýnda recourse_basket aldým ordan kontrol edecek
    opt.RequireHttpsMetadata = false;
});//74 basket mýcroservýste kullanýcýlý baglantýlýrdýr

//services.AddControllers();//74 alttaký gýbý genýsletýyoruz  koruma alýtýna alýyoruz yaný gýrýs sartý var
builder.Services.AddControllers(opt =>
{
    opt.Filters.Add(new AuthorizeFilter(requireAuthorizePolicy));//74 burayý genýsletýyoruz ve butun kontroller  token alamdan bagalanamz,user þartý var  requireAuthorizePolicy bunu yukarda tanýmladýk
});









var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
app.UseAuthentication();//74 gýrýs ýcýn kýmlýkdogrulama
app.UseAuthorization();

app.MapControllers();

app.Run();



