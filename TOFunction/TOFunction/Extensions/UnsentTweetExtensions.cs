using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace TOFunction.Services.DatabaseService
{
    public static class UnsentTweetExtensions
    {
        public static List<Tweet> MapFromDtos(this List<TweetDto> unsentTweetDtos)
        {
            List<Tweet> unsentTweets = new List<Tweet>();

            foreach (TweetDto tweetDto in unsentTweetDtos)
            {
                unsentTweets.Add(new Tweet
                {
                    Category = tweetDto.Category,
                    Deadline = tweetDto.Deadline,
                    Location = tweetDto.Location,
                    ScheduleTime = tweetDto.ScheduleTime
                });
            }

            return unsentTweets;
        }

        public static TweetTableEntity MapToTableEntity(this Tweet tweet)
            =>  new TweetTableEntity(tweet.Category, tweet.Location, tweet.Deadline,  tweet.ScheduleTime);        

        public static List<string> GetTweetsForTimeOfDay(this List<Tweet> tweetsToSend, ScheduleTime timeFilter) 
            => tweetsToSend.Where(x => x.ScheduleTime == timeFilter)
            .Select(tweet => JsonConvert.SerializeObject(tweet))
            .ToList();        

        public static string GeneratePublishableTweet(this Tweet unsentTweet) 
            => $"#TweetAlert - New Alert - \"<Cat/Type>\" - required - <Location> - Deadline <Date> - Don't miss out - You can Register Free with us for Alerts tailored to you! <WebsiteUrl>";
        

        public static List<List<string>> GetTweetsForSchedules(this List<Tweet> tweetsToSend)
        {
            List<List<string>> allTweets = new List<List<string>>();
            allTweets.Add(tweetsToSend.GetTweetsForTimeOfDay(ScheduleTime.Morning));
            allTweets.Add(tweetsToSend.GetTweetsForTimeOfDay(ScheduleTime.Midday));
            allTweets.Add(tweetsToSend.GetTweetsForTimeOfDay(ScheduleTime.Afternoon));
            return allTweets;
        }
    }
}
