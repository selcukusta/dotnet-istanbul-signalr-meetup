using DotnetIstanbulSignalRMeetup.Helpers;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace DotnetIstanbulSignalRMeetup.Hubs
{
    public interface ITwitterHub
    {
        Task SendTweet(string message);
    }
    public class TwitterHub : Hub<ITwitterHub>
    {
        public async Task Start()
        {
            await TwitterHelper.StartStream();
        }
    }
}
