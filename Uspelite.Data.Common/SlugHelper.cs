namespace Uspelite.Data.Common
{
    using System;
    using System.Text.RegularExpressions;


    public class SlugHelper
    {
        public static string CreateSlug(string title, int length = 100)
        {
            var substringLength = Math.Min(title.Length, length);
            var substring = title.Substring(0, substringLength);
            string cleanTitle = substring.ToLower().Replace(" ", "-");
            cleanTitle = Regex.Replace(cleanTitle, @"[^а-яА-Яa-zA-Z0-9_+ -]", "");
            return cleanTitle;
        }
    }
}
