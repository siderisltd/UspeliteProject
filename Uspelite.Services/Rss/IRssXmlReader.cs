namespace Uspelite.Services.Data.Rss
{
    using System.Collections.Generic;
    using DTO;

    public interface IRssXmlReader
    {
        IList<RssDTO> ReadRss(string url);
    }
}
