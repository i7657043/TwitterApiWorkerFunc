using System.Collections.Generic;
using System.Threading.Tasks;

namespace TOFunction.Services.DatabaseService
{
    public interface IDatabaseRepository
    {
        public Task<List<TweetDto>> CreateTweetsAsnyc();
    }
}
