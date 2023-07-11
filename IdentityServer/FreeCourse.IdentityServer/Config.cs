// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace FreeCourse.IdentityServer//38 bu sınıfı yapılandırıyz 
{
    public static class Config
    {
        public static IEnumerable<ApiResource> ApiResources => new ApiResource[]
        {//bunu bız ekledk
            new ApiResource("resourse_catalog"){Scopes={"catalog_fullpermission"}},//38
            new ApiResource("resourse_photo_stock"){Scopes={"photo_stock_fullpermission"}},//38
            new ApiResource("resourse_basket"){Scopes={"basket_fullpermission"}},//62
            new ApiResource("resourse_discount"){Scopes={"discount_fullpermission"}},//74           
            new ApiResource("resourse_order"){Scopes={"order_fullpermission"}},//96
            new ApiResource("resourse_fakepayment"){Scopes={"fakepayment_fullpermission"}},//101
            new ApiResource("resourse_gateway"){Scopes={"gateway_fullpermission"}},//109
            new ApiResource(IdentityServerConstants.LocalApi.ScopeName)
        };

        public static IEnumerable<IdentityResource> IdentityResources =>
                   new IdentityResource[]
                   {//38 önce ıcını sıldık 43 te doldurduk
                      new IdentityResources.Email(),
                      new IdentityResources.OpenId(),//mutlaka gonderılmesı gerekır
                      new IdentityResources.Profile(),
                      new IdentityResource()
                      {
                          Name="roles",
                          DisplayName="Roles",
                          Description="Kullanıcı rolleri",
                          UserClaims=new[]{"role"}
                      }
                   };

        public static IEnumerable<ApiScope> ApiScopes => new ApiScope[]
            {//38 ekledık içini önce sıldık..
                new ApiScope("catalog_fullpermission","Catalog API için ful erişim"),//38
                new ApiScope("photo_stock_fullpermission","Photo Stock API için ful erişim"),//38
                new ApiScope("basket_fullpermission","Basket  API için ful erişim"),//62
                new ApiScope("discount_fullpermission","Discount  API için ful erişim"),//74               
                new ApiScope("order_fullpermission","Order  API için ful erişim"),//96
                new ApiScope("fakepayment_fullpermission","FakePayment  API için ful erişim"),//101
                new ApiScope("gateway_fullpermission","Gateway  API için ful erişim"),//109
                new ApiScope(IdentityServerConstants.LocalApi.ScopeName)//38 identit kendısı ıcnde teyıt alıyor
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {//38 çnce içini sildik
               
                new Client//kullanıcı bılgısı sartı yok token..AllowedScopes ızınlı olan mıcroservısler
                {
                    ClientName="Asp.Net Core MVC",
                    ClientId="WebMvcClient",
                    ClientSecrets ={ new Secret("secret".Sha256()) },
                    AllowedGrantTypes=GrantTypes.ClientCredentials,
                    AllowedScopes={ "catalog_fullpermission","photo_stock_fullpermission", "gateway_fullpermission", IdentityServerConstants.LocalApi.ScopeName}//109 guncelledık
                },
                 new Client//43 ekledı kullanıcı bılgısı sart olan kullanıcı..AllowedScopes ızınlı olan mıcroservısler
                {
                    ClientName="Asp.Net Core MVC",
                    ClientId="WebMvcClientForUser",
                    AllowOfflineAccess=true,
                    ClientSecrets ={ new Secret("secret".Sha256()) },
                    AllowedGrantTypes=GrantTypes.ResourceOwnerPassword,
                    AllowedScopes={                          
                           "basket_fullpermission","discount_fullpermission","order_fullpermission","fakepayment_fullpermission","gateway_fullpermission",
                         IdentityServerConstants.StandardScopes.Email,
                         IdentityServerConstants.StandardScopes.OpenId,
                         IdentityServerConstants.StandardScopes.Profile,
                         IdentityServerConstants.StandardScopes.OfflineAccess,
                         IdentityServerConstants.LocalApi.ScopeName,"roles"},
                    AccessTokenLifetime=1*60*60,
                    RefreshTokenExpiration=TokenExpiration.Absolute,
                    AbsoluteRefreshTokenLifetime=(int)(DateTime.Now.AddDays(60)-DateTime.Now).TotalSeconds,
                    RefreshTokenUsage=TokenUsage.ReUse
                },
               

            };
    }
}