using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOFunction.Services.DatabaseService
{
    public class DatabaseService : IDatabaseService
    {
        private readonly IDatabaseProvider _databaseProvider;

        public DatabaseService(IDatabaseProvider databaseProvider)
        {
            _databaseProvider = databaseProvider;
        }

        public Task<List<UnsentTweet>> CreateTweetsAsync()
        {
            return _databaseProvider.CreateTweetsAsync();
        }
    }

    public class DatabaseProvider : IDatabaseProvider
    {
        private readonly IDatabaseRepository _databaseRepo;

        public DatabaseProvider(IDatabaseRepository databaseRepo)
        {
            _databaseRepo = databaseRepo;
        }

        public async Task<List<UnsentTweet>> CreateTweetsAsync()
        {
            List<UnsentTweetDto> unsentTweetDtos = await _databaseRepo.CreateTweetsAsnyc();

            return unsentTweetDtos.MapFromDtos();
        }
    }

    public class DatabaseRepository : IDatabaseRepository
    {
        private readonly string _dbConString;    
        
        public DatabaseRepository(IOptions<DatabaseCredentials> dbOptions)
        {
            _dbConString = dbOptions.Value.DbConnectionString;
        }

        public async Task<List<UnsentTweetDto>> CreateTweetsAsnyc()
        {
            return new List<UnsentTweetDto>
            {
                new UnsentTweetDto
                {
                    Field = "Morn_A",
                    ScheduleTime = ScheduleTime.Morning
                },
                new UnsentTweetDto
                {
                    Field = "Morn_B",
                    ScheduleTime = ScheduleTime.Morning
                },
                new UnsentTweetDto
                {
                    Field = "Mid_A",
                    ScheduleTime = ScheduleTime.Midday
                },
                new UnsentTweetDto
                {
                    Field = "Mid_B",
                    ScheduleTime = ScheduleTime.Midday
                },
                new UnsentTweetDto
                {
                    Field = "Aft_A",
                    ScheduleTime = ScheduleTime.Afternoon
                },
                new UnsentTweetDto
                {
                    Field = "Aft_B",
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
