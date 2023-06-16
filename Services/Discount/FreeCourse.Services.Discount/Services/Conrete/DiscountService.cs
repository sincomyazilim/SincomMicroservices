using Dapper;
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
            var deletedDiscount=await _dbconnection.ExecuteAsync("delete from discount where id=@Id",new { Id=id });
            return deletedDiscount > 0 ? ResponseDto<NoContent>.Success(204) : ResponseDto<NoContent>.Fail("indirim kodu bulunmadı", 404);
        }

        public async Task<ResponseDto<List<Models.Discount>>> GetAll()
        {//dapper kodu ıle getall yapıyoıruz
            var discounts = await _dbconnection.QueryAsync<Models.Discount>("Select *from discount");
            return ResponseDto<List<Models.Discount>>.Success(discounts.ToList(), 200);
        }

        public async Task<ResponseDto<Models.Discount>> GetByCodeAndUserId(string code, string userId)
        {
            //var discounts=await _dbconnection.QueryAsync<Models.Discount>("select * from discount where userid@UserId and code=@Code", new 
            //{
            //    Code=code,
            //    UserId=userId 
            //});

            var discounts = await _dbconnection.QueryAsync<Models.Discount>("select * from discount where userid=@UserId and code=@Code", new { UserId = userId, Code = code });
            var hasdiscount = discounts.FirstOrDefault();
            if (hasdiscount == null)
            {
                return ResponseDto<Models.Discount>.Fail("böyle indirim kodu yok", 404);                   

            }
            return ResponseDto<Models.Discount>.Success(hasdiscount,200);
        }

        public async Task<ResponseDto<Models.Discount>> GetById(int id)
        {
            var discount = (await _dbconnection.QueryAsync<Models.Discount>("select *from discount where id=@Id", new { Id = id })).SingleOrDefault();
            if (discount == null)
            {
                return ResponseDto<Models.Discount>.Fail("indirim bulunamadı", 404);
            }
            return ResponseDto<Models.Discount>.Success(discount, 200);
        }

        public async Task<ResponseDto<NoContent>> Save(Models.Discount discount)
        {
            var saveStatus = await _dbconnection.ExecuteAsync("INSERT INTO discount(userid,rate,code)VALUES(@UserId,@Rate,@Code)", discount);// burda kendısı maplame ozellıgı oldugu ıcın virgülden sonra discount koduk 
            if (saveStatus > 0)
            {
                return ResponseDto<NoContent>.Success(204);
            }
            return ResponseDto<NoContent>.Fail("Eklenemedi veritabna ayaktamı", 500);
        }

        public async Task<ResponseDto<NoContent>> Update(Models.Discount discount)
        {
            

                //var updateStatus = await _dbconnection.ExecuteAsync("UPDATE discount SET userId@UserId,code=@Code,rate=@Rate where id=@Id", new
                //{
                //    Id = discount.Id,
                //    UserId = discount.UserId,
                //    Code = discount.Code,
                //    Rate = discount.Rate
                //});// burda kendımmız maplemye bırakmadan kendımız eslestırdık.. ıstedıgımızı kullaanbılırız

            var updateStatus = await _dbconnection.ExecuteAsync("update discount set userid=@UserId, code=@Code, rate=@Rate where id=@Id", new { Id = discount.Id, UserId = discount.UserId, Code = discount.Code, Rate = discount.Rate });
            if (updateStatus > 0)
                {
                    return ResponseDto<NoContent>.Success(204);
                }
                
            
            return ResponseDto<NoContent>.Fail("Güncelnecek indirim codu yok", 404);
        }
    }
}
