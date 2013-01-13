using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MarkdownBlog.Services
{
    public static class Helpers
    {
        public static string Truncate(this string item, int truncateLength)
        {
            string truncatedString = item;

            if (item.Length > truncateLength)
            {
                truncatedString = item.Substring(0, truncateLength);
                truncatedString += "...";
            }

            return truncatedString;
        }
    }
}