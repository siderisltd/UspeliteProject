namespace Uspelite.Web.Models.Images
{
    using System.Collections.Generic;
    using Infrastructure.Mapping.Contracts;
    using Services.Data.DTO;

    public class PageableImageViewModel : IMapFrom<PagedImageDTO>
    {
        public IEnumerable<ImageViewModel> Images { get; set; }

        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public int AllItemsCount { get; set; }

        public int PageSize { get; set; }

        public int DisplayPageFrom { get; set; }

        public int DisplayPageTo { get; set; }
    }
}