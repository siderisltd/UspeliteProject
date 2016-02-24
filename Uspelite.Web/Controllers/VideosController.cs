namespace Uspelite.Web.Controllers
{
    using System;
    using System.Web.Mvc;
    using Models.Videos;
    using Services.Data.Contracts;

    public class VideosController : BaseController
    {
        private const int PAGE_SIZE = 15;
        private readonly IVideosService videosService;

        public VideosController(IVideosService videosService)
        {
            this.videosService = videosService;
        }

        [HttpGet]
        public ActionResult Index(int page = 1, int pageSize = PAGE_SIZE)
        {
            var dto = this.videosService.AllPaged(page, pageSize);
            var model = this.Mapper.Map<PageableVideoViewModel>(dto);
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