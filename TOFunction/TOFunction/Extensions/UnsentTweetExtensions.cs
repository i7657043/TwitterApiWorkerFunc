using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace TOFunction.Services.DatabaseService
{
    public static class UnsentTweetExtensions
    {
        public static List<UnsentTweet> MapFromDtos(this List<UnsentTweetDto> unsentTweetDtos)
        {
            List<UnsentTweet> unsentTweets = new List<UnsentTweet>();

            foreach (UnsentTweetDto tweetDto in unsentTweetDtos)
            {
                unsentTweets.Add(new UnsentTweet
                {
                    Field = tweetDto.Field,
                    ScheduleTime = tweetDto.ScheduleTime
                });
            }

            return unsentTweets;
        }

        public static List<string> GetTweetsForTimeOfDay(this List<UnsentTweet> tweetsToSend, ScheduleTime timeFilter)
        {
            return tweetsToSend.Where(x => x.ScheduleTime == timeFilter)
                .Select(tweet => JsonConvert.SerializeObject(tweet))
                .ToList();
        }
    }
}
