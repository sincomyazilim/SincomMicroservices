using FreeCourse.Shared.Services.Abstract;
using FreeCourse.Shared.Services.Concrete;
using FreeCourse.Web.Handler;
using FreeCourse.Web.Helpers;
using FreeCourse.Web.Models;
using FreeCourse.Web.Services.Abstract;
using FreeCourse.Web.Services.Concrete;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace FreeCourse.Web.Extensions//154
{
    public static class ServiceExtension
    {
        public static void AddHttpClientServices(this IServiceCollection services, IConfiguration Configuration)
        {
            
            var serviceApiSettings = Configuration.GetSection("ServiceApiSettings").Get<ServiceApiSettings>();//123 

            services.AddScoped<ISharedIdentityService, SharedIdentityService>();//134 bunu tanılıyoruz ıdentıty userId almak ıcın            

            services.AddScoped<IClientCredentialTokenService, ClientCredentialTokenService>();//135 ClientCredentialTokenService tanımlıyoruz 
            



            services.AddHttpClient<IEgitmenAdiGetirmek, EgitmenAdiGetirmek>(opt=>{//ben ekledım

                opt.BaseAddress = new Uri(serviceApiSettings.IdentityBaseUrl);
            }).AddHttpMessageHandler<ClientCredentialTokenHandler>();

           









            services.AddHttpClient<IOrderService, OrderService>(opt =>//174 servisler eklenırken htppclıne olarak ıstek yaptıklarında 
            {//scope olark degıl  services.AddHttpClient<IOrderService, OrderService> şeklınde eklenır.burdan appsettın içine gırıyor ıcınde serviceApiSettings bolumu ,boulum ıcınden Basket,ve onunda ıcınden path secılıyor

                opt.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseUrl}/{serviceApiSettings.Order.Path}");
            }).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();// order cagrıldgında token kendısı yetkısını gösterecek 
            //user bılgısı sarı var




            services.AddHttpClient<IFakePaymentService, FakePaymentService>(opt =>//171 servisler eklenırken htppclıne olarak ıstek yaptıklarında 
            {//scope olark degıl  services.AddHttpClient<IFakePaymentService, FakePaymentService> şeklınde eklenır.burdan appsettın içine gırıyor ıcınde serviceApiSettings bolumu ,boulum ıcınden Basket,ve onunda ıcınden path secılıyor

                opt.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseUrl}/{serviceApiSettings.FakePayment.Path}");
            }).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();// basket cagrıldgında token kendısı yetkısını gösterecek 
            //user bılgısı sarı var



            services.AddHttpClient<IDiscountService, DiscountService>(opt =>//154 servisler eklenırken htppclıne olarak ıstek yaptıklarında 
            {//scope olark degıl  services.AddHttpClient<IBasketService, BasketService> şeklınde eklenır.burdan appsettın içine gırıyor ıcınde serviceApiSettings bolumu ,boulum ıcınden Basket,ve onunda ıcınden path secılıyor

                opt.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseUrl}/{serviceApiSettings.Discount.Path}");
            }).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();// basket cagrıldgında token kendısı yetkısını gösterecek 
            //user bılgısı sarı var




            //services.AddHttpClient<IDiscountService, DiscountService>(opt =>//154 servisler eklenırken htppclıne olarak ıstek yaptıklarında 
            //{//scope olark degıl  services.AddHttpClient<IBasketService, BasketService> şeklınde eklenır.burdan appsettın içine gırıyor ıcınde serviceApiSettings bolumu ,boulum ıcınden Basket,ve onunda ıcınden path secılıyor

            //    opt.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseUrl}/{serviceApiSettings.Discount.Path}");
            //}).AddHttpMessageHandler<ClientCredentialTokenHandler>();// basket cagrıldgında token kendısı yetkısını gösterecek 
            ////user bılgısı sarı var    not bu kursa her kursa ayrı ayrı ındırım kodu ekleme olsun

            services.AddHttpClient<IBasketService22, BasketService22>(opt =>//154 servisler eklenırken htppclıne olarak ıstek yaptıklarında 
            {//scope olark degıl  services.AddHttpClient<IBasketService, BasketService> şeklınde eklenır.burdan appsettın içine gırıyor ıcınde serviceApiSettings bolumu ,boulum ıcınden Basket,ve onunda ıcınden path secılıyor

                opt.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseUrl}/{serviceApiSettings.Basket.Path}");
            }).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();// basket cagrıldgında token kendısı yetkısını gösterecek 
            //user bılgısı sarı var



            services.AddHttpClient<IBasketService, BasketService>(opt =>//154 servisler eklenırken htppclıne olarak ıstek yaptıklarında 
            {//scope olark degıl  services.AddHttpClient<IBasketService, BasketService> şeklınde eklenır.burdan appsettın içine gırıyor ıcınde serviceApiSettings bolumu ,boulum ıcınden Basket,ve onunda ıcınden path secılıyor

                opt.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseUrl}/{serviceApiSettings.Basket.Path}");
            }).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();// basket cagrıldgında token kendısı yetkısını gösterecek 
            //user bılgısı sarı var




            services.AddHttpClient<IPhotoStockService, PhotoStockService>(opt =>//143 servisler eklenırken htppclıne olarak ıstek yaptıklarında 
            {//scope olark degıl  services.AddHttpClient<IPhotoStockService, PhotoStockService> şeklınde eklenır.burdan appsettın içine gırıyor ıcınde serviceApiSettings bolumu ,boulum ıcınden catalog,ve onunda ıcınden path secılıyor

                opt.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseUrl}/{serviceApiSettings.PhotoStock.Path}");
            }).AddHttpMessageHandler<ClientCredentialTokenHandler>();// photostock cagrıldgında token kendısı yetkısını gösterecek 





            services.AddHttpClient<ICatalogService, CatalogService>(opt =>//132 servisler eklenırken htppclıne olarak ıstek yaptıklarında 
            {//scope olark degıl  services.AddHttpClient<ICatalogService, CatalogService> şeklınde eklenır.burdan appsettın içine gırıyor ıcınde serviceApiSettings bolumu ,boulum ıcınden catalog,ve onunda ıcınden path secılıyor

                opt.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseUrl}/{serviceApiSettings.Catalog.Path}");
            }).AddHttpMessageHandler<ClientCredentialTokenHandler>();// catalog cagrıldgında token kendısı yetkısını gösterecek category ve course görebılırmıyım göremzmıyım 136




            services.AddHttpClient<IUserService, UserService>(opt =>//123 userService cagrıldıgında baseurl gonder ve token baslıga ekle gonder
            {
                opt.BaseAddress = new Uri(serviceApiSettings.IdentityBaseUrl);//appsetingtekı verılerı okumak ıcın ve ıcındekı url almak ve UserService cagrıldıgında otomatık http://localhost:5001/ bu lınkı eklıyor basına UserService sınıfına bak getuser medudana acıklama yaptm
            }).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();//123 --125 AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>(); ekledık cunku bu ekledıgımız ıdentıtyserver ıstek sorgu yanı gıtıgınde basnıa tkne ekleyen sınıftır delagettır
        }



    }
}
//busınıf statup yogun olmasın dıye ek bır sınıf yapıp butun tanımlamalrı buray yapıyorum..sonra bunun ısmını sturtup eklıyoruz..dıkat edııelcek husuu bu sınıf mutlaka statıc ve kurulacak butun metotlarda statıc olacak
