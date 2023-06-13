using FreeCourse.Services.Catalog.Dtos.CategorieDto;
using FreeCourse.Shared.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FreeCourse.Services.Catalog.Services.Abstract
{
    public interface ICategoryService
    {
        Task<ResponseDto<List<CategoryDto>>> GetAllAsync();
        Task<ResponseDto<CategoryDto>> GetByIdAsync(string id);
        Task<ResponseDto<CategoryDto>> CreateCategoryAsync(CategoryCreateDto categoryCreateDto);
        Task<ResponseDto<NoContent>> UpdateCategoryAsync(CategoryUpdateDto categoryUpdateDto);
        Task<ResponseDto<NoContent>> DeleteCategoryAsync(string id);
    }
}
