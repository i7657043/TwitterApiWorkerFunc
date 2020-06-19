using System.Collections.Generic;
using System.Threading.Tasks;

namespace TOFunction.Services.DatabaseService
{
    public interface IDatabaseService
    {
        public Task<List<Tweet>> CreateTweetsAsync();
    }
}
