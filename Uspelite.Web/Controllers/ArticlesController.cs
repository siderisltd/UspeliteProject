namespace Uspelite.Web.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using Infrastructure.Mapping.Contracts;
    using Models.Articles;
    using Models.Home;
    using Services.Data.Contracts;

    //TODO: Rename to ArticlesController and every post stuffs also
    public class ArticlesController : BaseController
    {
        private readonly IArticlesService articlesService;

        public ArticlesController(IArticlesService articlesService)
        {
            this.articlesService = articlesService;
        }

        public ActionResult Show(string slug)
        {
            var model = this.articlesService.GetBySlug(slug).To<ConcreteArticleViewModel>().FirstOrDefault();
            return this.View(model);
        }
    }
}