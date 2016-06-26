namespace Uspelite.Services.Data
{
    using System;
    using System.Linq;
    using Contracts;
    using DTO;
    using Uspelite.Data.Models;
    using Uspelite.Data.Repositories;

    public class VideosService : IVideosService
    {
        private readonly IRepository<Video> repo;
        private readonly ICategoriesService categoriesService;

        public VideosService(IRepository<Video> repo, ICategoriesService categoriesService)
        {
            this.repo = repo;
            this.categoriesService = categoriesService;
        }

        public IQueryable<Video> All()
        {
            return this.repo.All();
        }

        public PagedVideoDTO AllPaged(int page, int pageSize, int? categoryId = null)
        {
            var dto = new PagedVideoDTO();
            var allVideosCount = this.repo.All().Count();
            dto.AllItemsCount = allVideosCount;
            dto.CurrentPage = page;
            dto.TotalPages = (int)Math.Ceiling(allVideosCount / (decimal)pageSize);

            var itemsToSkip = (page - 1) * pageSize;

            var query = this.repo.All();
          
            if (categoryId != null)
            {
                query = query.Where(x => x.CategoryId == categoryId);
            }

            var videos = query
                .OrderByDescending(x => x.CreatedOn)
                .Skip(itemsToSkip)
                .Take(pageSize)
                .AsQueryable();

            dto.Videos = videos;

            return dto;
        }

        public void SaveChanges()
        {
            this.repo.SaveChanges();
        }

        public void Add(Video videoAsDbModel)
        {
            this.repo.Add(videoAsDbModel);
        }
    }
}

//var dto = new PagedVideoDTO();
//var allVideosCount = this.repo.All().Count();
//dto.AllItemsCount = allVideosCount;
//            dto.CurrentPage = page;
//            dto.TotalPages = (int)Math.Ceiling(allVideosCount / (decimal)pageSize);

//            var itemsToSkip = (page - 1) * pageSize;

//var videos = this.repo
//                 .All()
//                 .OrderByDescending(z => z.CreatedOn)
//                 .Skip(itemsToSkip)
//                 .Take(pageSize)
//                 .AsQueryable();

//dto.Videos = videos;