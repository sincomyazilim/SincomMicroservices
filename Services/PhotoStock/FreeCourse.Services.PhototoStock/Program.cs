using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);





builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
{
    opt.Authority = builder.Configuration["IdentityServerURL"];
    opt.Audience = "resourse_photo_stock";//ýdentityserver ýcýndeký confýd dosyasýnda photo_stock_catalog aldým ordan kontrol edecek
    opt.RequireHttpsMetadata = false;
});//50





//services.AddControllers();//50 alttaký gýbý genýsletýyoruz
builder.Services.AddControllers(opt =>
{
    opt.Filters.Add(new AuthorizeFilter());//50 burayý genýsletýyoruz ve butun kontroller  token alamdan bagalanamz,user sartý yok
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();
app.UseAuthentication();//50 logýn ýcýn gýsý
app.UseAuthorization();

app.MapControllers();

app.Run();
