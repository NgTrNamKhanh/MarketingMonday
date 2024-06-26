﻿using AutoMapper;
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
            CreateMap<Article, ArticleResponse>();
            CreateMap<Article, SubmissionResponse>();

            //Event
            CreateMap<Event, EventDTO>();
            CreateMap<EventDTO, Event>();
            CreateMap<CreateAccountDTO, Account>();
            CreateMap<EditAccountDTO, Account>();

            // Comment
            CreateMap<Comment, CommentDTO>();
            CreateMap<CommentDTO, Comment>();
            CreateMap<Comment, CommentResponse>();
            CreateMap<EditCommentDTO, Comment>();
            //CreateMap<Comment, ReplyDTO>();
            //CreateMap<ReplyDTO, Comment>();

            //Likes and Dislikes
            CreateMap<ArticleInteractDTO,Like>();
            CreateMap<ArticleInteractDTO, Dislike>();
            CreateMap<CommentInteractDTO,Like>();
            CreateMap<CommentInteractDTO, Dislike>();
            CreateMap<Like,InteractResponse>();
            CreateMap<Dislike,InteractResponse>();

            //Notification
            CreateMap<Notification, NotificationResponse>();
            CreateMap<NotificationResponse, Notification>();
        }
    }
}
