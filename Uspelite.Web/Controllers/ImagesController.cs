namespace Uspelite.Web.Controllers
{
    using System.Web.Mvc;
    using Models.Images;
    using Services.Data.Contracts;

    public class ImagesController : BaseController
    {
        private const int PAGE_SIZE = 32;

        private IImagesService imagesService;

        public ImagesController(IImagesService imagesService)
        {
            this.imagesService = imagesService;
        }

        [HttpGet]
        public ActionResult Gallery(int page = 1, int pageSize = PAGE_SIZE)
        {
            var dto = this.imagesService.AllPaged(page, pageSize);
            var model = this.Mapper.Map<PageableImageViewModel>(dto);

            model.PageSize = pageSize;

            if (page <= 6)
            {
                model.DisplayPageFrom = 1;
                model.DisplayPageTo = 10 > model.TotalPages ? model.TotalPages : 10;
            }
            else if (page > 6)
            {
                var displayTo = page + 4;
                model.DisplayPageTo = model.TotalPages <= displayTo ? model.TotalPages : displayTo;
                model.DisplayPageFrom = model.DisplayPageTo - 9;
            }
            return this.View(model);
        }
    }
}