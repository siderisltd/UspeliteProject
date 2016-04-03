namespace Uspelite.Services.Data.Rss
{
    using System.Collections.Generic;
    using DTO;

    public class RssService : IRssService
    {
        private IRssXmlReader reader;
        private const string newsBgUrl = "http://newsbg.eu/feed/";
        private const string frogUrl = "http://rss.frognews.bg/0";
        private const string darikNews = "http://dariknews.bg/rss.php";
        private const string offNews = "http://offnews.bg/rss/all";
        private const string standartNews = "http://www.standartnews.com/rss.php?p=1";

        public RssService(IRssXmlReader reader)
        {
            this.reader = reader;
        }

        public IList<RssDTO> GetAllFrogNews()
        {
            var resultNews = this.reader.ReadRss(frogUrl);
            foreach (var item in resultNews)
            {
                item.ImageUrl = this.ParseImageStringFrogNews(item.Description);
                item.Description = this.ParseDescriptionFrogNews(item.Description);
            }
            return resultNews;
        }

        public IList<RssDTO> GetAllNewsBg()
        {
            var resultNews = this.reader.ReadRss(newsBgUrl);
            return resultNews;
        }

        public IList<RssDTO> GetOffNewsBg()
        {
            var resultNews = this.reader.ReadRss(offNews);
            return resultNews;
        }

        public IList<RssDTO> GetStandartNewsBg()
        {
            var resultNews = this.reader.ReadRss(standartNews);
            return resultNews;
        }

        public IList<RssDTO> GetAllDarikNews()
        {
            var resultNews = this.reader.ReadRss(darikNews);
            foreach (var item in resultNews)
            {
                item.ImageUrl = this.ParseImageStringDarik(item.Description);
                item.Description = this.ParseDescriptionDarik(item.Description);
            }
            return resultNews;
        }

        private string ParseDescriptionDarik(string description)
        {
            var len = description.Length;
            var result = string.Empty;
            for (int i = 0; i < len; i++)
            {
                if (description[i] == '>')
                {

                    result = description.Substring(i + 1, len - i - 1);
                }
            }

            return result;
        }

        private string ParseImageStringDarik(string description)
        {
            var result = string.Empty;
            var splittedDescs = description.Split(' ');
            var imgString = splittedDescs[1];
            var len = imgString.Length;
            result = imgString.Substring(5, len - 5).TrimEnd('"');
            return result;
        }

        private string ParseImageStringFrogNews(string description)
        {
            var result = string.Empty;
            var splittedRes = description.Split(' ');
            var imgSplit = splittedRes[2];
            var len = imgSplit.Length;
            result = imgSplit.Substring(5, len - 5).TrimEnd('"');
            return result;
        }

        private string ParseDescriptionFrogNews(string description)
        {
            var result = string.Empty;
            var splittedRes = description.Split(' ');
            for (int i = 0; i < splittedRes.Length; i++)
            {
                result += splittedRes[i];
            }
            var len = result.Length;
            return result;
        }
    }
}
