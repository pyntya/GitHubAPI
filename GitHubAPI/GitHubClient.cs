using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace GitHubAPI
{
    public class GitHubClient
    {
        readonly GitHubHttpClient client;

        public GitHubClient(AuthenticationHeaderValue authentication) => client = new GitHubHttpClient(authentication);

        public async Task<HttpResponseMessage> SendGetRequest(string uri) => await client.GetAsync(uri);
    }
}
