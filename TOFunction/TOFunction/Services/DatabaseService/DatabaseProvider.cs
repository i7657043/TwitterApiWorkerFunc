using System.Collections.Generic;
using System.Threading.Tasks;

namespace TOFunction.Services.DatabaseService
{
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
}
