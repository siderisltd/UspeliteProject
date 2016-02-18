namespace Uspelite.Web.Areas.Administration.Controllers
{
    using System.Web.Mvc;
    using Services.Data.Rss;

    public class RssController : AdministratorController
    {
        private readonly IRssService rssService;

        public RssController(IRssService rssService)
        {
            this.rssService = rssService;
        }

        public ActionResult Index()
        {
            return this.View();
        }

        public ActionResult GetNewsBgFeed()
        {
            var items = this.rssService.GetAllNewsBg();
            return this.PartialView("_FeedResults", items);
        }

        public ActionResult GetFrogNewsFeed()
        {
            var items = this.rssService.GetAllFrogNews();
            return this.PartialView("_FeedResults", items);
        }

        public ActionResult GetDarikNewsFeed()
        {
            var items = this.rssService.GetAllDarikNews();
            return this.PartialView("_FeedResults", items);
        }

        public ActionResult GetOffNewsFeed()
        {
            var items = this.rssService.GetOffNewsBg();
            return this.PartialView("_FeedResults", items);
        }

        public ActionResult GetStandartNewsFeed()
        {
            var items = this.rssService.GetStandartNewsBg();
            return this.PartialView("_FeedResults", items);
        }
    }
}