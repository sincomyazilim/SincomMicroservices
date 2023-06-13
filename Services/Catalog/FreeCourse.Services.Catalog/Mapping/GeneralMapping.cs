using AutoMapper;
using FreeCourse.Services.Catalog.Dtos.CategorieDto;


using FreeCourse.Services.Catalog.Dtos.CoursesDto;
using FreeCourse.Services.Catalog.Dtos.FeatureeDto;
using FreeCourse.Services.Catalog.Models;

namespace FreeCourse.Services.Catalog.Mapping
{
    public class GeneralMapping:Profile//21 maplama yapıyoruz
    {
        public GeneralMapping()
        {
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Category, CategoryCreateDto>().ReverseMap();
            CreateMap<Category, CategoryUpdateDto>().ReverseMap();

            CreateMap<Course,CourseDto>().ReverseMap();       
            CreateMap<Course, CourseCreateDto>().ReverseMap();
            CreateMap<Course, CourseUpdateDto>().ReverseMap();


            CreateMap<Feature, FeatureDto>().ReverseMap();
        }
    }
}
