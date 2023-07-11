using Dapper;
using FreeCourse.Services.Discount.Dtos;
using FreeCourse.Services.Discount.Services.Abstract;
using FreeCourse.Shared.Dtos;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCourse.Services.Discount.Services.Conrete//71
{
    public class DiscountService : IDiscountService
    {//PostgreSql verıtabanın baglantı sadece bu sınıf ııcndır egerkı genel ıstenııyorsa startup ıcınde yaızlması gerekır
        private readonly IConfiguration _configuration;
        private readonly IDbConnection _dbconnection;

        public DiscountService(IConfiguration configuration)
        {
            _configuration = configuration;
            _dbconnection = new NpgsqlConnection(_configuration.GetConnectionString("PostgreSql"));
        }
        //------------------------------------------------------------------------------------------------
        public async Task<ResponseDto<NoContent>> Delete(int id)
        {
            var deletedDiscount=await _dbconnection.ExecuteAsync("delete from discounteski where id=@Id",new { Id=id });
            return deletedDiscount > 0 ? ResponseDto<NoContent>.Success(204) : ResponseDto<NoContent>.Fail("indirim kodu bulunmadı", 404);
        }

        public async Task<ResponseDto<List<Models.DiscountEski>>> GetAll()
        {//dapper kodu ıle getall yapıyoıruz
            var discounts = await _dbconnection.QueryAsync<Models.DiscountEski>("Select *from discounteski");
            return ResponseDto<List<Models.DiscountEski>>.Success(discounts.ToList(), 200);
        }

        public async Task<ResponseDto<Models.DiscountEski>> GetByCodeAndUserId(string code, string userId)
        {
            

            var discounts = await _dbconnection.QueryAsync<Models.DiscountEski>("select * from discounteski where userid=@UserId and code=@Code", new { UserId = userId, Code = code });
           
            var hasdiscount = discounts.FirstOrDefault();
            if (hasdiscount == null)
            {
                return ResponseDto<Models.DiscountEski>.Fail("böyle indirim kodu yok", 404);                   

            }
            return ResponseDto<Models.DiscountEski>.Success(hasdiscount,200);
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------
        


        public async Task<ResponseDto<Models.DiscountEski>> GetById(int id)
        {
            var discount = (await _dbconnection.QueryAsync<Models.DiscountEski>("select *from discounteski where id=@Id", new { Id = id })).SingleOrDefault();
            if (discount == null)
            {
                return ResponseDto<Models.DiscountEski>.Fail("indirim bulunamadı", 404);
            }
            return ResponseDto<Models.DiscountEski>.Success(discount, 200);
        }

        public async Task<ResponseDto<NoContent>> Save(Models.DiscountEski discount)
        {
            var saveStatus = await _dbconnection.ExecuteAsync("INSERT INTO discounteski(userid,rate,code)VALUES(@UserId,@Rate,@Code)", discount);// burda kendısı maplame ozellıgı oldugu ıcın virgülden sonra discount koduk 
            if (saveStatus > 0)
            {
                return ResponseDto<NoContent>.Success(204);
            }
            return ResponseDto<NoContent>.Fail("Eklenemedi veritabanına, ayaktamı?", 500);
        }

        public async Task<ResponseDto<NoContent>> Update(Models.DiscountEski discount)
        {
            

                //var updateStatus = await _dbconnection.ExecuteAsync("UPDATE discount SET userId@UserId,code=@Code,rate=@Rate where id=@Id", new
                //{
                //    Id = discount.Id,
                //    UserId = discount.UserId,
                //    Code = discount.Code,
                //    Rate = discount.Rate
                //});// burda kendımmız maplemye bırakmadan kendımız eslestırdık.. ıstedıgımızı kullaanbılırız

            var updateStatus = await _dbconnection.ExecuteAsync("update discounteski set userid=@UserId, code=@Code, rate=@Rate where id=@Id", new { Id = discount.Id, UserId = discount.UserId, Code = discount.Code, Rate = discount.Rate });
            if (updateStatus > 0)
                {
                    return ResponseDto<NoContent>.Success(204);
                }
                
            
            return ResponseDto<NoContent>.Fail("Güncelenecek indirim Kodu yok", 404);
        }
    }
}
