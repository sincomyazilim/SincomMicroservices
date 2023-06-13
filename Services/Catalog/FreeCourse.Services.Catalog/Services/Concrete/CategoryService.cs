using AutoMapper;
using FreeCourse.Services.Catalog.Dtos.CategorieDto;
using FreeCourse.Services.Catalog.Dtos.CoursesDto;
using FreeCourse.Services.Catalog.Models;
using FreeCourse.Services.Catalog.Services.Abstract;
using FreeCourse.Services.Catalog.Settings.Abstract;
using FreeCourse.Shared.Dtos;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FreeCourse.Services.Catalog.Services.Concrete//24
{
    public class CategoryService: ICategoryService
    {
        private readonly IMongoCollection<Category> _categoriesCollection;
        private readonly IMapper _mapper;

        public CategoryService(IMapper mapper, IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);//mongoya bagantı ıcın clınet olsuturduk
            var database = client.GetDatabase(databaseSettings.DatabaseName);//ılgılıdatabse getırdık
            _categoriesCollection = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);//daatbasede ılgılı tabloyu yanı collection getırdık baglandık
            _mapper = mapper;
        }
        //-------------------------------------------------------------

        public async Task<ResponseDto<List<CategoryDto>>> GetAllAsync()
        {
            var categories = await _categoriesCollection.Find(Catalog => true).ToListAsync();
            return  ResponseDto<List<CategoryDto>>.Success(_mapper.Map<List<CategoryDto>>(categories), 200);
        }
       
        public async Task<ResponseDto<CategoryDto>> GetByIdAsync(string id)
        {
            var category=await _categoriesCollection.Find<Category>(x =>x.Id==id).FirstOrDefaultAsync();
            if (category==null)
            {
                return ResponseDto<CategoryDto>.Fail("Kategori bulunamadı", 404);
            }
            return ResponseDto<CategoryDto>.Success(_mapper.Map<CategoryDto>(category),200);
        } 
        public async Task<ResponseDto<CategoryDto>> CreateCategoryAsync(CategoryCreateDto categoryCreateDto)
        {
            var newCategory = _mapper.Map<Category>(categoryCreateDto);
            await _categoriesCollection.InsertOneAsync(newCategory);
            return ResponseDto<CategoryDto>.Success(_mapper.Map<CategoryDto>(newCategory), 200);
        }
        public async Task<ResponseDto<NoContent>> UpdateCategoryAsync(CategoryUpdateDto categoryUpdateDto)
        {
            var updateCategory = _mapper.Map<Category>(categoryUpdateDto);
            var result = await _categoriesCollection.FindOneAndReplaceAsync(x => x.Id == updateCategory.Id, updateCategory);

            if (result == null)
            {
                return ResponseDto<NoContent>.Fail("Bu kategori kayıtlı degıl", 404);
            }
            return ResponseDto<NoContent>.Success(204);
        }

        public async Task<ResponseDto<NoContent>> DeleteCategoryAsync(string id)
        {
            var result = await _categoriesCollection.DeleteOneAsync(x => x.Id == id);
            if (result.DeletedCount > 0)
            {
                return ResponseDto<NoContent>.Success(204);
            }
            else
            {
                return ResponseDto<NoContent>.Fail("silinecek kategori bulunmadı", 404);
            }
        }
    }
}
