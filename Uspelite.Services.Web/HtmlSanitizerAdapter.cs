namespace Uspelite.Services.Web
{
    using Contracts;
    using Ganss.XSS;

    public class HtmlSanitizerAdapter : ISanitizer
    {
        public string Sanitize(string html)
        {
            var sanitizer = new HtmlSanitizer();
            sanitizer.AllowedTags.Add("iframe");
            var result = sanitizer.Sanitize(html);
            return result;
        }
    }
}
