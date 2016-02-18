namespace Uspelite.Web.Areas.Administration.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using Models.Crawlers;
    using Services.Data.Crawl;
    using Microsoft.AspNet.Identity;
    using Infrastructure.Mapping.Contracts;
    using Services.Data.DTO;
    using Web.Models.Home;

    public class CrowlersController : AdministratorController
    {
        const int PARSE_PORTION_COUNT = 30000;

        private readonly ICrawlService crawlNewsService;

        public CrowlersController(ICrawlService crawlNewsService)
        {
            this.crawlNewsService = crawlNewsService;
        }

        public ActionResult Index()
        {
            var model = new CrowlerViewModel();
            model.Crawlers = this.crawlNewsService.GetAllPossibilities().ToList();
            return this.View(model);
        }

        public ActionResult StartPopulation(string Crawlers)
        {
            this.crawlNewsService.UpdateCount(int.Parse(Crawlers));
            return this.View(int.Parse(Crawlers));
        }

        public void StartPopulate(string id)
        {
            var userId = this.User.Identity.GetUserId();
            this.crawlNewsService.UpdateCount(int.Parse(id), PARSE_PORTION_COUNT);
            this.crawlNewsService.ParseNews(int.Parse(id), userId, PARSE_PORTION_COUNT);
        }

        public ActionResult GetParsedNewsCount(string id)
        {
            var count = this.crawlNewsService.GetCount(int.Parse(id));

            if(this.Session["passedItems"] == null)
            {
                this.Session["passedItems"] = 0;
            }
            else
            {
                this.Session["passedItems"] = int.Parse(this.Session["passedItems"].ToString()) + 80;
            }


            var passedItems = this.Session["passedItems"].ToString();
            var result = string.Format("Successfully parsed : {0}    From : {1}", count, passedItems);

            return this.Content(result);
        }
    }
}