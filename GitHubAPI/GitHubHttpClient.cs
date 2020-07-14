using System;
using System.Net.Http;

namespace GitHubAPI
{
    public class GitHubHttpClient : HttpClient
    {
        public static string GitHubUri { get; } = "https://api.github.com";

        public GitHubHttpClient(System.Net.Http.Headers.AuthenticationHeaderValue authentication)
        {
            DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            DefaultRequestHeaders.AcceptCharset.Add(new System.Net.Http.Headers.StringWithQualityHeaderValue("utf-8"));
            DefaultRequestHeaders.Authorization = authentication;
            DefaultRequestHeaders.UserAgent.Add(new System.Net.Http.Headers.ProductInfoHeaderValue(new System.Net.Http.Headers.ProductHeaderValue(AppDomain.CurrentDomain.FriendlyName)));
            BaseAddress = new Uri(GitHubUri);
        }
    }
}
