using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace DotnetIstanbulSignalRMeetup.Hubs
{
    public class ChatHub : Hub
    {
        public async Task Send(string message)
        {
            var formattedMessage = $"({Context.ConnectionId}) > {message}";
            await Clients.All.SendAsync("SendMessage", formattedMessage);
        }
    }
}
