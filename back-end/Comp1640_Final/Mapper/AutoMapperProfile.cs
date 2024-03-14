using AutoMapper;
using Comp1640_Final.DTO;
using Comp1640_Final.Models;

namespace Comp1640_Final.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ListArticleDTO, Article>();
            CreateMap<Article, ArticleDTO>();
            CreateMap<Event, EventDTO>();
            CreateMap<EventDTO, Event>();
        }
    }
}
