using System;
using System.Collections.Generic;
using System.Text;

namespace FreeCourse.Shared.Services.Abstract//59
{
    public interface ISharedIdentityService
    {
        public string GetUserId { get; }
    }
}
//bu sınıf shred tanımladık ıdentıty server ıkıde baglanp userıd ıstemek ıcın bu katmandakı porej uzerınden baglanıp
//userId cagırıyoruz uyelık gerektıre n mıkroservıslerde kullanıcı mutlaka gırtısyapacagı ıcın userıd sart olan 
//mıcroservıosler burdan userıd cagırabılecek