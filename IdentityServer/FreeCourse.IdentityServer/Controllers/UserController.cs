using FreeCourse.IdentityServer.Dtos;
using FreeCourse.IdentityServer.Models;
using FreeCourse.Shared.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using static IdentityServer4.IdentityServerConstants;

namespace FreeCourse.IdentityServer.Controllers//37
{
    [Authorize(LocalApi.PolicyName)]//40 logın olma sartı
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpClientFactory _httpClientFactory;

        public UserController(UserManager<ApplicationUser> userManager, IHttpClientFactory httpClientFactory)
        {
            _userManager = userManager;
            _httpClientFactory = httpClientFactory;
        }
        //-----------------------------------------------------------------------
        [HttpPost]
        public async Task<IActionResult> Signup(SignupDto signupDto)
        {
            
            var user = new ApplicationUser //automapper kullanamdıgım ıcın bu yöntem
            {
                UserName = signupDto.UserName,
                Email = signupDto.Email,
                Sehir = signupDto.City
            };
            var result = await _userManager.CreateAsync(user, signupDto.Password);
            if (!result.Succeeded)
            {
                return BadRequest(ResponseDto<NoContent>.Fail(result.Errors.Select(x => x.Description).ToList(), 400));
            }
            return NoContent();
        }

        [HttpGet("GetUser")]
        public async Task<IActionResult> GetUser()//45 burda user hakkında bılgılerı getırıyroz
        {
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub);
            if (userIdClaim == null)
            {
                return BadRequest();
            }
            var user = await _userManager.FindByIdAsync(userIdClaim.Value);
            if (user == null)
            {
                return BadRequest();
            }
            return Ok(new { Id = user.Id, UserName = user.UserName, Email = user.Email, Sehir = user.Sehir });
        }


        [HttpGet("GetAllUser")]
       
        public async Task<IActionResult> GetAllUser()//ben ekeldım userIdye göre ıd cekme
        {
            var users = await _userManager.Users.ToListAsync();
            if (users == null)
            {
                return BadRequest();
            }

            return Ok(users);
        }
    }
}
