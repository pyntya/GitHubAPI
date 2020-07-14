using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GitHubAPI
{
    public class GitHubApiEndpointsContext
    {
        private Dictionary<string, string> _endpointsDictionary;

        public GitHubApiEndpointsContext(Dictionary<string, string> endpointsDictionary)
        {
            _endpointsDictionary = endpointsDictionary;
        }

        public int GetEndpointsCount()
        {
            return _endpointsDictionary.Count;
        }

        public IEnumerable<string> GetCategories()
        {
            var pattern = @$"{GitHubHttpClient.GitHubUri}\/?(?<sectionName>\w*)";
            return _endpointsDictionary.Select(x => Regex.Match(x.Value, pattern).Groups["sectionName"].Value);
        }

        public IEnumerable<IGrouping<string, string>> GetGroupedCategories()
        {
            return GetCategories().GroupBy(x => x);
        }

        public IOrderedEnumerable<IGrouping<string, string>> GetGroupedCategoriesOrderedByCountDescending()
        {
            return GetGroupedCategories().OrderByDescending(x => x.Count());
        }

        public IEnumerable<KeyValuePair<string, string>> GetQueries()
        {
            return _endpointsDictionary.Where(x => x.Value.Contains("{query}"));
        }
    }
}
