namespace Uspelite.Data.Common
{
    using System;
    using System.Text.RegularExpressions;


    public class SlugHelper
    {
        public static string CreateSlug(string title, int length = 50)
        {
            var substringLength = Math.Min(title.Length - 1, length);
            var substring = title.Substring(0, substringLength + 1);
            string cleanTitle = substring.ToLower().Replace(" ", "-");
            cleanTitle = Regex.Replace(cleanTitle, @"[^а-яА-Яa-zA-Z0-9_+ -]", "");
            return cleanTitle;
        }
    }
}
