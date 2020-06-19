using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using TOFunction.Models;

namespace TOFunction.Extensions
{
    public static class UnsentTweetExtensions
    {
        public static List<CustomTweet> MapFromDtos(this List<CustomTweetDto> unsentTweetDtos)
        {
            List<CustomTweet> unsentTweets = new List<CustomTweet>();

            foreach (CustomTweetDto tweetDto in unsentTweetDtos)
            {
                unsentTweets.Add(new CustomTweet
                {
                    Category = tweetDto.Category,
                    Deadline = tweetDto.Deadline,
                    Location = tweetDto.Location,
                    ScheduleTime = tweetDto.ScheduleTime
                });
            }

            return unsentTweets;
        }

        public static CustomTweetTableEntity MapToTableEntity(this CustomTweet tweet)
            =>  new CustomTweetTableEntity(tweet.Category, tweet.Location, tweet.Deadline,  tweet.ScheduleTime);        

        public static List<string> GetTweetsForTimeOfDay(this List<CustomTweet> tweetsToSend, ScheduleTime timeFilter) 
            => tweetsToSend.Where(x => x.ScheduleTime == timeFilter)
            .Select(tweet => JsonConvert.SerializeObject(tweet))
            .ToList();        

        public static string GeneratePublishableTweet(this CustomTweet unsentTweet) 
            => $"#TweetAlert - New Alert - \"<Cat/Type>\" - required - <Location> - Deadline <Date> - Don't miss out - You can Register Free with us for Alerts tailored to you! <WebsiteUrl>";
        

        public static List<List<string>> GetTweetsForSchedules(this List<CustomTweet> tweetsToSend)
        {
            List<List<string>> allTweets = new List<List<string>>();
            allTweets.Add(tweetsToSend.GetTweetsForTimeOfDay(ScheduleTime.Morning));
            allTweets.Add(tweetsToSend.GetTweetsForTimeOfDay(ScheduleTime.Midday));
            allTweets.Add(tweetsToSend.GetTweetsForTimeOfDay(ScheduleTime.Afternoon));
            return allTweets;
        }
    }
}
