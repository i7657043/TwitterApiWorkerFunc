using System.Collections.Generic;
using System.Threading.Tasks;
using TOFunction.Models;

namespace TOFunction.Services
{
    public interface IDatabaseRepository
    {
        public Task<List<CustomTweetDto>> CreateTweetsAsnyc();
    }
}
