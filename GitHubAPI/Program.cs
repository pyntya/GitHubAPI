using GitHubAPI.Extensions;
using System;
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

            var context = new GitHubApiContext(authentication);

            Console.WriteLine(GetNumberOfEndpointsReultString(context));
            Console.WriteLine(GetCategoriesOrderedByCountDescendingResultString(context));
            Console.WriteLine(GetQueriesResultString(context));
        }

        private static string GetNumberOfEndpointsReultString(GitHubApiContext context) => $"1. Number of endpoints: {context.GetEndpointsCount()}";
        private static string GetCategoriesOrderedByCountDescendingResultString(GitHubApiContext context) => $"2. {string.Join(", ", context.GetGroupedCategoriesOrderedByCountDescending().Select(x => $"{x.Key}: {x.Count()}"))}";
        private static string GetQueriesResultString(GitHubApiContext context) => $"3. {string.Join(Environment.NewLine, context.GetQueries().Select(x => x.Value))}";
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
