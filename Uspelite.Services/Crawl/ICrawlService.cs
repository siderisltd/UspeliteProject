namespace Uspelite.Services.Data.Crawl
{
    using System.Linq;
    using DTO;
    using Uspelite.Data.Models.Enum;

    public interface ICrawlService
    {
        IQueryable<CrowlerDTO> GetAllPossibilities();

        int GetCount(int id);

        bool UpdateCount(int id, int count = 1000);

        void ParseNews(int id, string userId, int portionCount = 1000);
    }
}
