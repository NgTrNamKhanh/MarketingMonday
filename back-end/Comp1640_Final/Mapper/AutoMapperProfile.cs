using AutoMapper;
using Comp1640_Final.DTO.Request;
using Comp1640_Final.Models;

namespace Comp1640_Final.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AddArticleDTO, Article>();
            CreateMap<EditArticleDTO, Article>();
            CreateMap<Article, ArticleDTO>();
            CreateMap<Article, SubmissionDTO>();
            CreateMap<Event, EventDTO>();
            CreateMap<EventDTO, Event>();
        }
    }
}
