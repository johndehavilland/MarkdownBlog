using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownBlog.Services
{
    interface IBlogConvertor
    {
        /// <summary>
        /// Converts blog text to the required HTML output
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        string Convert(string input);
    }
}
