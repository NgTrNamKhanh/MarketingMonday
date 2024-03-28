using Comp1640_Final.Data;
using Comp1640_Final.Models;
using Microsoft.EntityFrameworkCore;

namespace Comp1640_Final.Services
{

    public interface INotificationService
    {
        Task<bool> PostNotification(Notification notification);

        Task<bool> Save();

    }

    public class NotificationService : INotificationService
    {
        private readonly ProjectDbContext _context;

        public NotificationService(ProjectDbContext context)
        {
            _context = context;
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
