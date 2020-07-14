using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace GitHubAPI
{
    public class GitHubApiEndpointsContext
    {
        private Dictionary<string, string> _endpointsDictionary;

        public GitHubApiEndpointsContext(Dictionary<string, string> endpointsDictionary) => _endpointsDictionary = endpointsDictionary;

        public int GetEndpointsCount() => _endpointsDictionary.Count;

        public IEnumerable<string> GetCategories()
        {
            var wordPattern = @$"\w*";
            return _endpointsDictionary.Select(x => Regex.Match(x.Value.Split("/")[3], wordPattern).Value);
        }

        public IEnumerable<IGrouping<string, string>> GetGroupedCategories() => GetCategories().GroupBy(x => x);

        public IOrderedEnumerable<IGrouping<string, string>> GetGroupedCategoriesOrderedByCountDescending() => GetGroupedCategories().OrderByDescending(x => x.Count());

        public IEnumerable<KeyValuePair<string, string>> GetQueries() => _endpointsDictionary.Where(x => x.Value.Contains("{query}"));
    }
}
