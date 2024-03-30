namespace Comp1640_Final.DTO.Response
{
    public class NotificationResponse
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
        public UserNoti UserNoti { get; set; }
    }

    public class UserNoti
    {
        public string Id { get; set; }
        public byte[] UserAvatar { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}
