namespace Uspelite.Web.Models.Videos
{
    using System.Collections.Generic;
    using Infrastructure.Mapping.Contracts;
    using Services.Data.DTO;

    public class PageableVideoViewModel : IMapFrom<PagedVideoDTO>
    {
        public IEnumerable<VideoViewModel> Videos { get; set; }

        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public int AllItemsCount { get; set; }

        public int PageSize { get; set; }

        public int DisplayPageFrom { get; set; }

        public int DisplayPageTo { get; set; }
    }
}