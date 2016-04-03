namespace Uspelite.Web.Models.Articles
{
    using AutoMapper;
    using Data.Models;
    using Infrastructure.Mapping.Contracts;

    public class BuildLinkArticleModel : IMapFrom<Article>
    {
        public string Title { get; set; }

        public string Slug { get; set; }
    }
}