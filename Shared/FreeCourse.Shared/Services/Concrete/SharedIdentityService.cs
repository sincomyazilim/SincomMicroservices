using FreeCourse.Shared.Services.Abstract;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FreeCourse.Shared.Services.Concrete//59
{
    public class SharedIdentityService : ISharedIdentityService
    {
        //bu ınterface bıze ıdentıtyserver uzerındekı context baglar contexte ıse zaten token oldugu ıcın token uzerınden cleanler yanı token ozellıklerı  ve bu ozellıkler ıcınde kayıtlı ola userId ulasabılırız bu sınıfları hangı mıcroservısete kullancaksak o mıcrosercıste  startup kısmına bunu cagırması ıcn bu  services.AddHttpContextAccessor();   kodu eklıyoruz
        private IHttpContextAccessor _httpContextAccessor;

        public SharedIdentityService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }


        //---------------------------------------------------------------------------------
        
        //public string GetUserId => _httpContextAccessor.HttpContext.User.Claims.Where(x=>x.Type=="Sub").FirstOrDefault().Value;//iki sekılde ulasabılırız
        public string GetUserId => _httpContextAccessor.HttpContext.User.FindFirst("sub").Value;
       
    }
}
