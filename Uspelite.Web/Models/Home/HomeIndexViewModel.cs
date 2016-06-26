namespace Uspelite.Web.Models.Home
{
    using System.Collections.Generic;
    using Articles;
    using Base;

    public class HomeIndexViewModel : PaginationModel
    {
        public IList<CategoriesAndArticlesViewModel> NewestPostsAndCategories { get; set; }

        public IList<ArticleViewModel> SearchResults { get; set; }

        public IList<ArticleViewModel> HighRatedPosts { get; set; }

        public IList<ArticleViewModel> MostCommentedPosts { get; set; }

        public IList<ArticleViewModel> HighRatedInCategory { get; set; }

        public IList<ArticleViewModel> NewestArticles { get; set; }

        public ArticleViewModel TopArticle { get; set; }

        public bool IsSearchResult { get; set; }
    }
}