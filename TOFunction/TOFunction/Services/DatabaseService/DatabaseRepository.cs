using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TOFunction.Services.DatabaseService
{
    public class DatabaseRepository : IDatabaseRepository
    {
        private readonly string _dbConString;    
        
        public DatabaseRepository(IOptions<DatabaseCredentials> dbOptions)
        {
            _dbConString = dbOptions.Value.DbConnectionString;
        }

        public async Task<List<TweetDto>> CreateTweetsAsnyc()
        {
            return new List<TweetDto>
            {
                new TweetDto
                {
                    Category = "Cat_MA",
                    Deadline = DateTime.Now,
                    Location = "Loc_A",
                    ScheduleTime = ScheduleTime.Morning
                },
                new TweetDto
                {
                    Category = "Cat_B",
                    Deadline = DateTime.Now,
                    Location = "Loc_B",
                    ScheduleTime = ScheduleTime.Morning
                },
                new TweetDto
                {
                    Category = "Cat_C",
                    Deadline = DateTime.Now,
                    Location = "Loc_C",
                    ScheduleTime = ScheduleTime.Midday
                },
                new TweetDto
                {
                    Category = "Cat_A",
                    Deadline = DateTime.Now,
                    Location = "Loc_A",
                    ScheduleTime = ScheduleTime.Midday
                },
                new TweetDto
                {
                    Category = "Cat_B",
                    Deadline = DateTime.Now,
                    Location = "Loc_A",
                    ScheduleTime = ScheduleTime.Afternoon
                },
                new TweetDto
                {
                    Category = "Cat_C",
                    Deadline = DateTime.Now,
                    Location = "Loc_C",
                    ScheduleTime = ScheduleTime.Afternoon
                }
            };
            
            //using (SqlConnection connection = new SqlConnection(_dbConString))
            //{
                // TODO: Need to map to schema, obviously
                //IEnumerable<UnsentTweetDto> unsentTweet = await connection.QueryAsync<UnsentTweetDto>("SELECT TOP 1 * FROM Tables WHERE Id = 1");

                //return unsentTweet.ToList();
            //}
        }
    }
}
