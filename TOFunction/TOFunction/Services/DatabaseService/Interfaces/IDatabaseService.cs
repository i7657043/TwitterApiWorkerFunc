using System.Collections.Generic;
using System.Threading.Tasks;
using TOFunction.Models;

namespace TOFunction.Services
{
    public interface IDatabaseService
    {
        public Task<List<CustomTweet>> CreateTweetsAsync();
    }
}
