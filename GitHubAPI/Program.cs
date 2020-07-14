using GitHubAPI.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace GitHubAPI
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Net.Http.Headers.AuthenticationHeaderValue authentication;

            if (args.Length == 1 && args[0] == "-h")
            {
                WriteHelpMessage();
                return;
            }
            else if (args.Length == 0)
                authentication = null;
            else if (args.Length == 1 && Enum.TryParse(typeof(AuthenticationTypes), args[0], true, out var authType) && (AuthenticationTypes)authType == AuthenticationTypes.NoAuthentication)
                authentication = null;

            else if (args.Length == 2 && Enum.TryParse(typeof(AuthenticationTypes), args[0], true, out var authenticationType))
            {
                authentication = (AuthenticationTypes)authenticationType switch
                {
                    AuthenticationTypes.Basic => new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", args[1].GetBase64EncodedString()),
                    AuthenticationTypes.OAuth2 => new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", args[1]),
                    _ => null
                };
            }
            else
            {
                Console.WriteLine(ErrorMessage);
                return;
            }

            var client = new GitHubClient(authentication);
            var message = client.SendGetRequest("/").Result;
            if (message.IsSuccessStatusCode)
            {
                var context = new GitHubApiEndpointsContext(JsonConvert.DeserializeObject<Dictionary<string, string>>(message.Content.ReadAsStringAsync().Result));
                Console.WriteLine(GetNumberOfEndpointsReultString(context.GetEndpointsCount()));
                Console.WriteLine(GetCategoriesOrderedByCountDescendingResultString(context.GetGroupedCategoriesOrderedByCountDescending()));
                Console.WriteLine(GetQueriesResultString(context.GetQueries()));
            }
            else
            {
                Console.WriteLine(message.ToString());
                return;
            }

            
        }

        private static string GetNumberOfEndpointsReultString(int count) => $"1. Number of endpoints: {count}";
        private static string GetCategoriesOrderedByCountDescendingResultString(IOrderedEnumerable<IGrouping<string, string>> categories) => $"2. {string.Join(", ", categories.Select(x => $"{x.Key}: {x.Count()}"))}";
        private static string GetQueriesResultString(IEnumerable<KeyValuePair<string, string>> queries) => $"3. {string.Join(Environment.NewLine, queries.Select(x => x.Value))}";
        private static string ErrorMessage => $"Wrong parameters. {HelpMessage}";
        private static string HelpMessage => "Possible parameter values:" + Environment.NewLine
            + "* no parameters for calling API without authentication" + Environment.NewLine
            + "* \"NoAuthentication\" parameter for calling API without authentication" + Environment.NewLine
            + "* \"Basic\" parameter followed by credentials in format \"user:password\" for calling API using 'Basic' authentication" + Environment.NewLine
            + "* \"OAuth2\" parameter followed by \"OAuth token\" for calling API using 'OAuth2' authentication";
        private static void WriteHelpMessage()
        {
            Console.WriteLine(HelpMessage);
        }
    }
}
