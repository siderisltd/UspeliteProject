namespace Uspelite.Services.Data.Rss
{
    using System.Collections.Generic;
    using DTO;

    public interface IRssService
    {
        IList<RssDTO> GetAllFrogNews();

        IList<RssDTO> GetAllNewsBg();

        IList<RssDTO> GetStandartNewsBg();

        IList<RssDTO> GetAllDarikNews();

        IList<RssDTO> GetOffNewsBg();
    }
}
