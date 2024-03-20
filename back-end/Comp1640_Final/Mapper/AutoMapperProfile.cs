using AutoMapper;
using Comp1640_Final.DTO.Request;
using Comp1640_Final.DTO.Response;
using Comp1640_Final.Models;

namespace Comp1640_Final.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //Article
            CreateMap<AddArticleDTO, Article>();
            CreateMap<EditArticleDTO, Article>();
            CreateMap<Article, ArticleDTO>();
            CreateMap<Article, SubmissionDTO>();

            //Event
            CreateMap<Event, EventDTO>();
            CreateMap<EventDTO, Event>();

            // Comment
            CreateMap<Comment, CommentDTO>();
            CreateMap<CommentDTO, Comment>();
            CreateMap<Comment, CommentResponse>();
            //CreateMap<Comment, ReplyDTO>();
            //CreateMap<ReplyDTO, Comment>();
        }
    }
}
