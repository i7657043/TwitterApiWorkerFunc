using Dapper;
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

        public Task<List<Tweet>> CreateTweetsAsync()
        {
            return _databaseProvider.CreateTweetsAsync();
        }
    }
}
