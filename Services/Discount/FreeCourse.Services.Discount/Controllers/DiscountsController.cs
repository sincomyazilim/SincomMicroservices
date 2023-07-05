using FreeCourse.Services.Discount.Dtos;
using FreeCourse.Services.Discount.Services.Abstract;
using FreeCourse.Shared.ControlerBase;
using FreeCourse.Shared.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FreeCourse.Services.Discount.Controllers//75
{
    [ApiVersion("1.0")]//bunu ekeldım api ye lınkte sınıf gnderek ıcın https://www.youtube.com/watch?v=x0W2cYy5tIw bunu uyguladım
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountsController : CustomBaseController
    {
        private readonly IDiscountService _discountService;
        private readonly ISharedIdentityService _sharedIdentityService;
        

        public DiscountsController(IDiscountService discountService, ISharedIdentityService sharedIdentityService)
        {
            _discountService = discountService;
            _sharedIdentityService = sharedIdentityService;
          
        }
        //-------------------------------------------------------------------------

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return CreateActionResultInstance(await _discountService.GetAll());
        }
       
        
        
        // localhost/api/discount/5
        [HttpGet("{id}")]
        public async Task<IActionResult>GetById(int id)
        {
            var discount=await _discountService.GetById(id);
            return CreateActionResultInstance(discount);
        }
       
        
        
        [HttpGet]
        [Route("/api/[controller]/[action]/{code}")]
        
        public async Task<IActionResult>GetByCode(string code)
        {
            var userId = _sharedIdentityService.GetUserId;
            var discount=await _discountService.GetByCodeAndUserId(code,userId);
            return CreateActionResultInstance(discount);
        }



       






        [HttpPost]
        public async Task<IActionResult> Save(Models.DiscountEski discount)
        {
            return CreateActionResultInstance(await _discountService.Save(discount));
        }
        
        [HttpPut]
        public async Task<IActionResult>Update(Models.DiscountEski discount)
        {
            return CreateActionResultInstance(await _discountService.Update(discount));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return CreateActionResultInstance(await _discountService.Delete(id));
        }
    }
}
