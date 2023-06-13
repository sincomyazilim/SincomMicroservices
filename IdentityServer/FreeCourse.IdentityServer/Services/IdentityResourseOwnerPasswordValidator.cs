﻿using FreeCourse.IdentityServer.Models;
using IdentityModel;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FreeCourse.IdentityServer.Services//42 bu sınıfla uyelık şartı ve giriş yapmıs olması gerekır zıretcı edılecek sınıflar ıcın dır
{
    public class IdentityResourseOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public IdentityResourseOwnerPasswordValidator(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        //-----------------------------------------------------------------
        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var existUser = await _userManager.FindByEmailAsync(context.UserName);//böyle bır kullancıı varmı
            if (existUser==null)
            {
                var errors=new Dictionary<string, object>();
                errors.Add("error",new List<string> { "Email adınız veya şifreniz yanlış"});
                context.Result.CustomResponse=errors;
                return;
            }
            var passwordCheck=await _userManager.CheckPasswordAsync(existUser, context.Password);//şifre dogrumu
            if (passwordCheck == false)
            {
                var errors = new Dictionary<string, object>();
                errors.Add("error", new List<string> { "Email adınız veya şifreniz yanlış" });
                context.Result.CustomResponse = errors;
                return;
            }
            context.Result = new GrantValidationResult(existUser.Id.ToString(), OidcConstants.AuthenticationMethods.Password);//giriş yapabılırısn
        }
    }
}