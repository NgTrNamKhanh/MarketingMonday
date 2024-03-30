using Comp1640_Final.Data;
using Comp1640_Final.Models;
using Microsoft.EntityFrameworkCore;

namespace Comp1640_Final.Services
{

    public interface INotificationService
    {
        Task<bool> PostNotification(Notification notification);
        Task<ICollection<Notification>> GetNotifications(string userId);
        Task<Notification> GetNotiByUserAndMessage(string userId, string message);
        Task<bool> DeleteNoti(Notification notification);
        Task<bool> Save();

    }

    public class NotificationService : INotificationService
    {
        private readonly ProjectDbContext _context;

        public NotificationService(ProjectDbContext context)
        {
            _context = context;
        }

        public async Task<bool> DeleteNoti(Notification notification)
        {
            _context.Remove(notification);
            return await Save();
        }

        public async Task<Notification> GetNotiByUserAndMessage(string userId, string message)
        {
            return await _context.Notifications.Where(n => n.UserId == userId).Where(n => n.Message == message).FirstOrDefaultAsync();
        }

        public async Task<ICollection<Notification>> GetNotifications(string userId)
        {
            return await _context.Notifications.Where(n => n.UserId == userId).OrderBy(n => n.CreatedAt).ToListAsync();
        }

        public async Task<bool> PostNotification(Notification notification)
        {
            _context.Notifications.Add(notification);
            return await Save();
        }

        public async Task<bool> Save()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }
    }
}
