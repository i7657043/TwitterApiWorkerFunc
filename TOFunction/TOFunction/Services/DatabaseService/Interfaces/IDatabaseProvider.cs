using System.Collections.Generic;
using System.Threading.Tasks;

namespace TOFunction.Services.DatabaseService
{
    public interface IDatabaseProvider
    {
        public Task<List<Tweet>> CreateTweetsAsync();
    }
}
