using System.Collections.Generic;
using System.Threading.Tasks;
using TOFunction.Models;

namespace TOFunction.Services
{ 
    public class DatabaseService : IDatabaseService
    {
        private readonly IDatabaseProvider _databaseProvider;

        public DatabaseService(IDatabaseProvider databaseProvider)
        {
            _databaseProvider = databaseProvider;
        }

        public Task<List<CustomTweet>> CreateTweetsAsync()
        {
            return _databaseProvider.CreateTweetsAsync();
        }
    }
}
