using AutoMapper;
using Comp1640.DTO;
using Comp1640.Models;

namespace Comp1640.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() {
            CreateMap<ArticleDTO, Article>();
            CreateMap<Article, ArticleDTO>();
        }
    }
}
