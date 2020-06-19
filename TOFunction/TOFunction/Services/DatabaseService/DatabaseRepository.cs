using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TOFunction.Data;
using TOFunction.Models;

namespace TOFunction.Services
{
    public class DatabaseRepository : IDatabaseRepository
    {
        private readonly string _dbConString;    
        
        public DatabaseRepository(IOptions<DatabaseCredentials> dbOptions)
        {
            _dbConString = dbOptions.Value.DbConnectionString;
        }

        public async Task<List<CustomTweetDto>> CreateTweetsAsnyc()
        {
            return GetFakeTweets();

            //using (SqlConnection connection = new SqlConnection(_dbConString))
            //{
            // TODO: Need to map to schema, obviously
            //IEnumerable<UnsentTweetDto> unsentTweet = await connection.QueryAsync<UnsentTweetDto>("SELECT TOP 1 * FROM Tables WHERE Id = 1");

            //return unsentTweet.ToList();
            //}
        }

        private static List<CustomTweetDto> GetFakeTweets()
        {
            return new List<CustomTweetDto>
            {
                new CustomTweetDto
                {
                    Category = "Cat_MA",
                    Deadline = DateTime.Now,
                    Location = "Loc_A",
                    ScheduleTime = ScheduleTime.Morning
                },
                new CustomTweetDto
                {
                    Category = "Cat_B",
                    Deadline = DateTime.Now,
                    Location = "Loc_B",
                    ScheduleTime = ScheduleTime.Morning
                },
                new CustomTweetDto
                {
                    Category = "Cat_C",
                    Deadline = DateTime.Now,
                    Location = "Loc_C",
                    ScheduleTime = ScheduleTime.Midday
                },
                new CustomTweetDto
                {
                    Category = "Cat_A",
                    Deadline = DateTime.Now,
                    Location = "Loc_A",
                    ScheduleTime = ScheduleTime.Midday
                },
                new CustomTweetDto
                {
                    Category = "Cat_B",
                    Deadline = DateTime.Now,
                    Location = "Loc_A",
                    ScheduleTime = ScheduleTime.Afternoon
                },
                new CustomTweetDto
                {
                    Category = "Cat_C",
                    Deadline = DateTime.Now,
                    Location = "Loc_C",
                    ScheduleTime = ScheduleTime.Afternoon
                }
            };
        }
    }
}
