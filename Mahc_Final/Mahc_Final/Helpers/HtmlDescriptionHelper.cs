using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace Mahc_Final.Helpers
{
    public class HtmlDescriptionHelper
    {
        private static string HtmlToPlainText(string html)
        {
            const string tagWhiteSpace = @"(>|$)(\W|\n|\r)+<";//matches one or more (white space or line breaks) between '>' and '<'
            const string stripFormatting = @"<[^>]*(>|$)";//match any character between '<' and '>', even when end tag is missing
            const string lineBreak = @"<(br|BR)\s{0,1}\/{0,1}>";//matches: <br>,<br/>,<br />,<BR>,<BR/>,<BR />
            var lineBreakRegex = new Regex(lineBreak, RegexOptions.Multiline);
            var stripFormattingRegex = new Regex(stripFormatting, RegexOptions.Multiline);
            var tagWhiteSpaceRegex = new Regex(tagWhiteSpace, RegexOptions.Multiline);

            var text = html;
            //Decode html specific characters
            text = System.Net.WebUtility.HtmlDecode(text);
            //Remove tag whitespace/line breaks
            text = tagWhiteSpaceRegex.Replace(text, "><");
            //Replace <br /> with line breaks
            text = lineBreakRegex.Replace(text, Environment.NewLine);
            //Strip formatting
            text = stripFormattingRegex.Replace(text, string.Empty);

            return text;
        }

        public static string GetShortDescFromHtml(string html, int count = 20)
        {
            var result = "";
            string plainText = HtmlToPlainText(html);

            string[] words = plainText.Split();
            result = string.Join(" ", words.Take(count));
            result += "...";
            return result;
        }

        public static string StripHTML(string input)
        {
            return Regex.Replace(input, "<.*?>", String.Empty);
        }
    }
}