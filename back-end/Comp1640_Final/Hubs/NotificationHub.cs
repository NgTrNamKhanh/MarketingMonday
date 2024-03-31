using Microsoft.AspNetCore.SignalR;

namespace Comp1640_Final.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task SendNotification(string userId ,string message)
        {
            //await Clients.User(userId).SendAsync("ReceiveNotification", message);
            await Clients.All.SendAsync("ReceiveNotification", message);

        }
    }
}
