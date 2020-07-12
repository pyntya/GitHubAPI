using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;

namespace GitHubAPI
{
    public class GitHubApi
    {
        static readonly HttpClient client = new GitHubHttpClient();
   
        public static Dictionary<string, string> GetApiList()
        {
            var response = client.GetAsync("/");
            var responseBody = response.Result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(responseBody.Result);
        }
    }
}
