namespace Uspelite.Web.Models.Home
{
    using System.Collections.Generic;
    using Articles;

    public class HomeIndexViewModel
    {
        public IList<CategoriesAndArticlesViewModel> NewestPostsAndCategories { get; set; }

        public IList<ArticleViewModel> SearchResults { get; set; }

        public IList<ArticleViewModel> HighRatedPosts { get; set; }

        public IList<ArticleViewModel> MostCommentedPosts { get; set; }

        public IList<ArticleViewModel> HighRatedInCategory { get; set; }

        public ArticleViewModel TopArticle { get; set; }

        public bool IsSearchResult { get; set; }

        public string Query { get; set; }

        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public int AllItemsCount { get; set; }

        public int PageSize { get; set; }

        public int DisplayPageFrom { get; set; }

        public int DisplayPageTo { get; set; }
    }
}