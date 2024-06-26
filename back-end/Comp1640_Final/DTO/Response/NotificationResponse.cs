﻿namespace Comp1640_Final.DTO.Response
{
    public class NotificationResponse
    {
        public int Id { get; set; }
        public Guid? ArticleId { get; set; }
        public Guid? CommentId { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
        public UserNoti UserNoti { get; set; }
        public bool IsRead { get; set; }

    }

    public class UserNoti
    {
        public string Id { get; set; }
        public string UserAvatar { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}
