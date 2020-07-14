using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GitHubAPI
{
    public class GitHubApiContext
    {
        readonly GitHubHttpClient client;
   
        public GitHubApiContext(AuthenticationHeaderValue authentication)
        {
            client = new GitHubHttpClient(authentication);
        }

        public async Task<T> SendGetRequest<T>(string uri)
        {
            var response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var responseBody = response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(responseBody.Result);
            }
            return default;
        }

        public int GetEndpointsCount()
        {
            return SendGetRequest<Dictionary<string, string>>("/").Result.Count;
        }

        public IEnumerable<string> GetCategories()
        {
            var pattern = @$"{GitHubHttpClient.GitHubUri}\/?(?<sectionName>\w*)";
            return SendGetRequest<Dictionary<string, string>>("/").Result.Select(x => Regex.Match(x.Value, pattern).Groups["sectionName"].Value);
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
            return SendGetRequest<Dictionary<string, string>>("/").Result.Where(x => x.Value.Contains("{query}"));
        }
    }
}
