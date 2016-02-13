namespace Uspelite.Web.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
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

        public ActionResult ShowCategory(string title)
        {
            var model = this.categoriesService.GetByTitle(title).To<CategoriesViewModel>().FirstOrDefault();
                            
            return this.View(model);
        }
    }
}