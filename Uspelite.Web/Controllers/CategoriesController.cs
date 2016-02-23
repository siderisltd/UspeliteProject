namespace Uspelite.Web.Controllers
{
    using System.Web.Mvc;
    using System.Linq;
    using Infrastructure.Mapping.Contracts;
    using Models.Categories;
    using Services.Data.Contracts;

    public class CategoriesController : BaseController
    {
        private readonly ICategoriesService categoriesService;

        public CategoriesController(ICategoriesService categoriesService)
        {
            this.categoriesService = categoriesService;
        }

        public ActionResult Show(string slug, int page = 1, int pageSize = 10)
        {
            var model = this.categoriesService.GetPaged(slug, page, pageSize).To<PageableCategoryArticlesViewModel>().FirstOrDefault();

            model.Slug = slug;
            model.PageSize = pageSize;

            if (page <= 6)
            {
                model.DisplayPageFrom = 1;
                model.DisplayPageTo = 10 > model.TotalPages ? model.TotalPages : 10;
            }
            else if(page > 6)
            {
                var displayTo = page + 4;
                model.DisplayPageTo = model.TotalPages <= displayTo? model.TotalPages : displayTo;
                model.DisplayPageFrom = model.DisplayPageTo - 9;
            }

            //model.TopArticle = model.Articles.FirstOrDefault();

            return this.View(model);
        }
    }
}