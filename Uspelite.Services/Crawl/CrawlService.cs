namespace Uspelite.Services.Data.Crawl
{
    using System;
    using System.Collections.Generic;
    using System.Drawing.Imaging;
    using System.Linq;
    using AngleSharp;
    using Contracts;
    using DTO;
    using Uspelite.Data.Models;
    using Uspelite.Data.Models.Enum;
    using Uspelite.Data.Repositories;

    public class CrawlService : ICrawlService
    {
        private readonly IRepository<CrawlNews> repo;

        private readonly IArticlesService articlesService;

        private readonly IImagesService imagesService;

        ICategoriesService categoriesService;

        public CrawlService(IRepository<CrawlNews> repo, IArticlesService articlesService, ICategoriesService categoriesService, IImagesService imagesService)
        {
            this.imagesService = imagesService;
            this.categoriesService = categoriesService;
            this.articlesService = articlesService;
            this.repo = repo;
        }

        public int GetCount(int id)
        {
            var crawl = this.repo.All().FirstOrDefault(x => x.Id == (CrawlType)id);
            if(crawl == null)
            {
                throw new ArgumentNullException("Crowl type not found [Searched Id]:" + (int)id);
            }

            return crawl.Count;
        }

        public bool UpdateCount(int id, int count = 1000)
        {
            var crawl = this.repo.All().FirstOrDefault(x => x.Id == (CrawlType)id);

            if (crawl == null)
            {
                throw new ArgumentNullException("Crowl type not found [Searched Id]:" + (int)id);
            }

            crawl.Count = crawl.Count + count;
            this.repo.SaveChanges();
            return true;
        }

        public IQueryable<CrowlerDTO> GetAllPossibilities()
        {
            var result = new List<CrowlerDTO>();


            foreach (CrawlType crawler in Enum.GetValues(typeof(CrawlType)))
            {
                result.Add(new CrowlerDTO
                {
                    Id = (int)crawler,
                    Count = this.repo.All().FirstOrDefault(x => x.Id == (CrawlType)crawler).Count,
                    Name = crawler.ToString()
                });
            }

            return result.AsQueryable();
        }
        
        public void ParseNews(int id, string userId, int portionCount = 1000)
        {
            var crawlCount = this.repo.All().FirstOrDefault(x => x.Id == (CrawlType)id).Count;

            switch ((CrawlType)id)
            {
                case CrawlType.DarikNews:
                    this.DarikNewsCrowler(id, crawlCount, userId, portionCount);
                    break;
                case CrawlType.NewsBg:
                    this.NewsBgCrawler(id, crawlCount, userId, portionCount);
                    break;
                case CrawlType.StandardNews:
                    this.StandardNewsCrawler(id, crawlCount, userId, portionCount);
                    break;
            }
        }

        private void StandardNewsCrawler(int id, int crawlCount, string userId, int portionCount)
        {
            throw new NotImplementedException();
        }

        private void NewsBgCrawler(int id, int crawlCount, string userId, int portionCount)
        {
            throw new NotImplementedException();
        }

        private void DarikNewsCrowler(int id, int from, string userId, int portionCount)
        {
            var configuration = Configuration.Default.WithDefaultLoader();
            var browsingContext = BrowsingContext.New(configuration);
            var to = from + portionCount;
            if (from % 2 == 0)
            {
                from += 1;
            }
            for (int i = from; i <= to; i+=2)
            {


                var url = string.Format("http://dariknews.bg/view_article.php?article_id={0}", i);
                //var url = "http://dariknews.bg/view_article.php?article_id=1552571";
                var document = browsingContext.OpenAsync(url).Result;

                var articleContent = document.QuerySelector("#textsize");
                if (articleContent != null)
                {
                    var title = document.QuerySelector("div.cbox h1").TextContent.Trim();

                    if (this.articlesService.Exists(title))
                    {
                        continue;
                    }
                    var content = articleContent.TextContent.Trim();
                    var isImageExists = document.QuerySelector("div.cbox.article.wt link");
                    if(isImageExists == null)
                    {
                        continue;
                    }
                    var imageUrl = "http://www.dariknews.bg/" + document.QuerySelector("div.cbox.article.wt link").GetAttribute("rel");
                    var categoryTitle = document.QuerySelector("p.nav").Children.Select(x => x.GetAttribute("title")).LastOrDefault();

                    bool isCategoryExists = this.categoriesService.GetAll().Any(x => x.Title == categoryTitle);

                    int categoryId;
                    if (!isCategoryExists)
                    {
                        categoryId = this.categoriesService.Add(new Category { Title = categoryTitle });
                    }
                    else
                    {
                        categoryId = this.categoriesService.GetAll().FirstOrDefault(x => x.Title == categoryTitle).Id;
                    }
                    
                    var picture = this.imagesService.SaveImageFromWeb(imageUrl, title, ImageFormat.Jpeg, userId);

                    var createdArticleId = this.articlesService.Add(title, userId, content, PostStatus.Draft, categoryId, picture);
                }
            }
            this.UpdateCount(id, to);
        }
    }
}
