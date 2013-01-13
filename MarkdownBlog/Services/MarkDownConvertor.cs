using MarkdownSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MarkdownBlog.Services
{
    public class MarkDownConvertor:IBlogConvertor
    {
        /// <summary>
        /// Converts markdown text to HTML
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string Convert(string input)
        {
            Markdown markDownService = new Markdown();
            string convertedText = markDownService.Transform(input);
            return convertedText;
        }
    }
}