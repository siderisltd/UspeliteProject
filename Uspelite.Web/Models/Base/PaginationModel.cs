namespace Uspelite.Web.Models.Base
{
    public class PaginationModel
    {
        public string Query { get; set; }

        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public int AllItemsCount { get; set; }

        public int PageSize { get; set; }

        public int DisplayPageFrom { get; set; }

        public int DisplayPageTo { get; set; }
    }
}