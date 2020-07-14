using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace GitHubAPI.Extensions
{
    public static class StringExtensions
    {
        public static string GetBase64EncodedString(this string str)
        {
            return Convert.ToBase64String(Encoding.ASCII.GetBytes(str));
        }
    }
}
