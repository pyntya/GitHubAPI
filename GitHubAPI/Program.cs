using System;
using System.Linq;

namespace GitHubAPI
{
    class Program
    {
        static void Main(string[] args)
        {
            var dictionary = GitHubApi.GetApiList();
            var categories = dictionary.Select(x => x.Value.Split("/")[3]).GroupBy(x => x).OrderByDescending(x => x.Count());
            var queries = dictionary.Where(x => x.Value.Contains("{query}"));

            Console.WriteLine($"1. Number of endpoints: {dictionary.Count}");
            Console.WriteLine($"2. {string.Join(", ", categories.Select(x => $"{x.Key}: {x.Count()}"))}");
            Console.WriteLine($"3. {string.Join(Environment.NewLine, queries.Select(x => x.Value))}");
        }
    }
}
