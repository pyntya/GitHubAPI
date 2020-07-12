using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace GitHubAPI
{
    public class GitHubHttpClient : HttpClient
    {
        static string gitHubUri = "https://api.github.com";

        public GitHubHttpClient()
        {
            DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            DefaultRequestHeaders.AcceptCharset.Add(new System.Net.Http.Headers.StringWithQualityHeaderValue("utf-8"));
            DefaultRequestHeaders.UserAgent.Add(new System.Net.Http.Headers.ProductInfoHeaderValue(new System.Net.Http.Headers.ProductHeaderValue("pyntya")));
            BaseAddress = new Uri(gitHubUri);
        }
    }
}
