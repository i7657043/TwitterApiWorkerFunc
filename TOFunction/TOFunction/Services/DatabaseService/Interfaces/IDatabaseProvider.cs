using System.Collections.Generic;
using System.Threading.Tasks;
using TOFunction.Models;

namespace TOFunction.Services
{
    public interface IDatabaseProvider
    {
        public Task<List<CustomTweet>> CreateTweetsAsync();
    }
}
