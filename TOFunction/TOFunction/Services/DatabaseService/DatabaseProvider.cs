using System.Collections.Generic;
using System.Threading.Tasks;
using TOFunction.Extensions;
using TOFunction.Models;

namespace TOFunction.Services
{
    public class DatabaseProvider : IDatabaseProvider
    {
        private readonly IDatabaseRepository _databaseRepo;

        public DatabaseProvider(IDatabaseRepository databaseRepo)
        {
            _databaseRepo = databaseRepo;
        }

        public async Task<List<CustomTweet>> CreateTweetsAsync()
        {
            List<CustomTweetDto> unsentTweetDtos = await _databaseRepo.CreateTweetsAsnyc();

            return unsentTweetDtos.MapFromDtos();
        }
    }
}
