using DotnetIstanbulSignalRMeetup.Hubs;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Streaming;

namespace DotnetIstanbulSignalRMeetup.Helpers
{
    public static class TwitterHelper
    {
        private static IFilteredStream _stream;
        public static IHubContext<TwitterHub, ITwitterHub> HubContext { get; set; }

        public static async Task StartStream()
        {
            var consumerKey = Environment.GetEnvironmentVariable("TWITTER_CONSUMER_KEY", EnvironmentVariableTarget.User);
            var consumerSecret = Environment.GetEnvironmentVariable("TWITTER_CONSUMER_SECRET", EnvironmentVariableTarget.User);
            var accessToken = Environment.GetEnvironmentVariable("TWITTER_ACCESS_TOKEN", EnvironmentVariableTarget.User);
            var accessTokenSecret = Environment.GetEnvironmentVariable("TWITTER_ACCESS_TOKEN_SECRET", EnvironmentVariableTarget.User);

            var credentials = Auth.SetUserCredentials(consumerKey, consumerSecret, accessToken, accessTokenSecret);
            if (_stream == null)
            {
                _stream = Stream.CreateFilteredStream(credentials);
                _stream.AddTrack("#WorldCup");
                _stream.MatchingTweetReceived += async (sender, args) =>
                {
                    await HubContext.Clients.All.SendTweet(args.Tweet.FullText);
                };
                await _stream.StartStreamMatchingAllConditionsAsync();
            }
            else
            {
                _stream.ResumeStream();
            }
        }
    }
}
