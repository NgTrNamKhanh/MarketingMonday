using AutoMapper;
using Comp1640_Final.Data;
using Comp1640_Final.DTO.Response;
using Comp1640_Final.Migrations;
using Comp1640_Final.Models;
using Comp1640_Final.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Comp1640_Final.Controllers
{
    [Route("api/notification")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly ProjectDbContext _context;
        private readonly IAritcleService _aritcleService;
        private readonly ILikeService _likeService;
        private readonly IDislikeService _dislikeService;
        private readonly ICommentService _commentService;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserService _userService;
        private static IWebHostEnvironment _webHostEnvironment;
        private readonly INotificationService _notificationService;


        public NotificationsController(ProjectDbContext context,
            IAritcleService aritcleService,
            ILikeService likeService,
            IDislikeService dislikeService,
            ICommentService commentService,
            IMapper mapper,
            UserManager<ApplicationUser> userManager,
            IUserService userService,
            IWebHostEnvironment webHostEnvironment,
            INotificationService notificationService)
        {
            _context = context;
            _aritcleService = aritcleService;
            _likeService = likeService;
            _dislikeService = dislikeService;
            _commentService = commentService;
            _mapper = mapper;
            _userManager = userManager;
            _userService = userService;
            _webHostEnvironment = webHostEnvironment;
            _notificationService = notificationService;
        }

        [HttpGet()]
        public async Task<IActionResult> GetNotifications(string userId)
        {
            var notifications = await _notificationService.GetNotifications(userId);
            if (notifications == null)
            {
                return BadRequest("Not found any notification");
            }
            var notiResponses = new List<NotificationResponse>();

            foreach (var notification in notifications)
            {
                var notiResponse = _mapper.Map<NotificationResponse>(notification);
                var user = await _userManager.FindByIdAsync(notification.UserInteractionId);
                var userImageBytes = await _userService.GetImagesByUserId(user.Id);

                if (userImageBytes == null)
                {
                    var defaultImageFileName = "default-avatar.jpg";
                    var defaultImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "UserAvatars", "DontHaveAva", defaultImageFileName);
                    userImageBytes = await System.IO.File.ReadAllBytesAsync(defaultImagePath);
                    
                }
                UserNoti userNoti = new UserNoti
                {
                    Id = user.Id,
                    UserAvatar = userImageBytes,
                    FirstName = user.FirstName,
                    LastName = user.LastName
                };
                notiResponse.UserNoti = userNoti;
                notiResponses.Add(notiResponse);
            }
            return Ok(notiResponses);
        }


        //[HttpGet("likeNoti/{articleId}")]
        //public async Task<IActionResult> GetArticleNotifications(Guid articleId)
        //{
        //    var likes = await _likeService.GetArticleLikes(articleId);
        //    var article =  _aritcleService.GetArticleByID(articleId);
        //    var notiResponses = new List<NotificationResponse>();
        //    //var notification = _context.Notifications.OrderBy(n => n.Id).ToList();
        //    //var notiResponses = _mapper.Map<List<NotificationResponse>>(notification);
        //    //var users = new List<ApplicationUser>();
        //    foreach (var like in likes)
        //    {
        //        var user = await _userManager.FindByIdAsync(like.UserId);
        //        var userImageBytes = await _userService.GetImagesByUserId(user.Id);
        //        if (userImageBytes == null)
        //        {
        //            var defaultImageFileName = "default-avatar.jpg";
        //            var defaultImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "UserAvatars", "DontHaveAva", defaultImageFileName);
        //            userImageBytes = await System.IO.File.ReadAllBytesAsync(defaultImagePath);
        //        }
        //        UserNoti userNoti = new UserNoti
        //        {
        //            Id = user.Id,
        //            UserAvatar = userImageBytes,
        //            FirstName = user.FirstName,
        //            LastName = user.LastName
        //        };
        //        NotificationResponse noti = new NotificationResponse
        //        {
        //            Message =  user.FirstName + " " + user.LastName + " liked your Post",
        //            UserNoti = userNoti
        //        };

        //        //noti.UserNoti = userNoti;
        //        notiResponses.Add(noti);
        //    }



        //    return Ok(notiResponses);

        //}
    }
}
