using AutoMapper;
using FreeCourse.Services.Catalog.Dtos.CoursesDto;
using FreeCourse.Services.Catalog.Models;
using FreeCourse.Services.Catalog.Services.Abstract;
using FreeCourse.Services.Catalog.Settings.Abstract;
using FreeCourse.Shared.Dtos;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCourse.Services.Catalog.Services.Concrete//25
{// conctor ıcınde yazılanlar mongodbye baglanıp ılgıl tablodan verı cekmek ıcın yapılıyro
    public class CourseService: ICourseService
    {
        private readonly IMongoCollection<Course> _courseCollection;//mongo db argumanları
        private readonly IMongoCollection<Category> _categoryCollection;
        private readonly IMapper _mapper;

        public CourseService(IMapper mapper, IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);//mongo db clınet olsutur ve baglan
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            _courseCollection = database.GetCollection<Course>(databaseSettings.CourseCollectionName);//course tablosuna baglan
            _categoryCollection = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);//category tbl baglan
            _mapper = mapper;
        }
        //--------------------------------------------------------------------------------

        public async Task<ResponseDto<List<CourseDto>>> GetAllAsync()
        {
            var courses = await _courseCollection.Find(course => true).ToListAsync();
            if (courses.Any())
            {
                foreach (var item in courses)
                {
                    item.Category = await _categoryCollection.Find(x => x.Id == item.CategoryId).FirstAsync();
                }
            }
            else
            {
                courses = new List<Course>();
            }
            return ResponseDto<List<CourseDto>>.Success(_mapper.Map<List<CourseDto>>(courses), 200);
        }

        public async Task<ResponseDto<CourseDto>> GetByIdAsync(string id)
        {
            var course = await _courseCollection.Find<Course>(x => x.Id == id).FirstOrDefaultAsync();
            if (course == null)
            {
                return ResponseDto<CourseDto>.Fail("Kurs kaydı bulunmadı", 404);
            }
            course.Category = await _categoryCollection.Find<Category>(x => x.Id == course.CategoryId).FirstAsync();
            return ResponseDto<CourseDto>.Success(_mapper.Map<CourseDto>(course), 200);
        }

        public async Task<ResponseDto<List<CourseDto>>> GetAllByUserIdAsync(string userId)
        {
            var courses = await _courseCollection.Find<Course>(x => x.UserId == userId).ToListAsync();
            if (courses.Any())
            {
                foreach (var item in courses)
                {
                    item.Category = await _categoryCollection.Find(x => x.Id == item.CategoryId).FirstAsync();
                }
            }
            else
            {
                courses = new List<Course>();
            }
            return ResponseDto<List<CourseDto>>.Success(_mapper.Map<List<CourseDto>>(courses), 200);
        }

        public async Task<ResponseDto<CourseDto>> CreateCourseAsync(CourseCreateDto courseCreateDto)
        {
            var newCouser = _mapper.Map<Course>(courseCreateDto);
            await _courseCollection.InsertOneAsync(newCouser);//mongeya göre eklnıyor
            return ResponseDto<CourseDto>.Success(_mapper.Map<CourseDto>(newCouser), 200);
        }
        public async Task<ResponseDto<NoContent>> UpdateCourseAsync(CourseUpdateDto courseUpdateDto)
        {
            var updateCourse=_mapper.Map<Course>(courseUpdateDto);
            var result = await _courseCollection.FindOneAndReplaceAsync(x => x.Id == updateCourse.Id, updateCourse);

            if (result==null)
            {
                return ResponseDto<NoContent>.Fail("Bukurs kayıtlı degıl", 404);
            }
            return ResponseDto<NoContent>.Success(204);
        }

        public async Task<ResponseDto<NoContent>>DeleteCourseAsync(string id)
        {
            var result=await _courseCollection.DeleteOneAsync(x=>x.Id == id);
            if (result.DeletedCount>0)
            {
                return ResponseDto<NoContent>.Success(204);
            }
            else
            {
                return ResponseDto<NoContent>.Fail("silinecek kurs bulunmadı", 404);
            }
        }
    }
}
