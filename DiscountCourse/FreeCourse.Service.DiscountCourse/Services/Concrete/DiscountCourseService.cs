using Dapper;
using FreeCourse.Service.DiscountCourse.Services.Abstract;
using FreeCourse.Shared.Dtos;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCourse.Service.DiscountCourse.Services.Concrete
{
    public class DiscountCourseService : IDiscountCourseService
    {
        private readonly IConfiguration _configuration;
        private readonly IDbConnection _dbconnection;

        public DiscountCourseService(IConfiguration configuration)
        {
            _configuration = configuration;
            _dbconnection = new NpgsqlConnection(_configuration.GetConnectionString("PostgreSql")); 
        }
        //--------------------------------------------------------------------------
        // tum seppetekı kurslara ındırım degıld kursa göre ındırım metodu
        public async Task<ResponseDto<Models.DiscountCourse>> GetByCodeAndUserIdAndCourseId(string code, string userId, string courseId)
        {            
                var discounts = await _dbconnection.QueryAsync<Models.DiscountCourse>("select * from discount1 where userid=@UserId and code=@Code and courseid=@CourseId", new { UserId = userId, Code = code, CourseId = courseId });
                var hasdiscount = discounts.FirstOrDefault();
                if (hasdiscount == null)
                {
                    return ResponseDto<Models.DiscountCourse>.Fail("böyle indirim kodu yok", 404);

                }
                return ResponseDto<Models.DiscountCourse>.Success(hasdiscount, 200);
            
        }
    }
}
