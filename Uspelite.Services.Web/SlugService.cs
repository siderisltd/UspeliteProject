namespace Uspelite.Services.Web
{
    using System;
    using System.Text.RegularExpressions;
    using Contracts;

    public class SlugService : ISlugService
    {
        public string GetSlug(string title, int length = 50)
        {
            var substringLength = Math.Min(title.Length - 1, length);
            var substring = title.Substring(substringLength);
            string cleanTitle = substring.ToLower().Replace(" ", "-");
            cleanTitle = Regex.Replace(cleanTitle, @"[^а-яА-Яa-zA-Z0-9_+ -]", "");
            return cleanTitle;
        }
    }
}
