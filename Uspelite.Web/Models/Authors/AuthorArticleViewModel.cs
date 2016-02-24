namespace Uspelite.Web.Models.Authors
{
    using System;
    using System.Linq;
    using Articles;
    using AutoMapper;
    using Common;
    using Data.Models;
    using Infrastructure.Mapping.Contracts;

    public class AuthorArticleViewModel : IRateable, IMapFrom<Article>, IHaveCustomMappings
    {
        private ArticleImageViewModel _mainArticlePic = new ArticleImageViewModel
        {
            Resized400Picture = "Content/Imgs/no-photo-available.jpg",
            IsMain = true,
            Title = "No photo available"
        };

        public int Id { get; set; }

        public int RateType { get { return (int)RateableType.Article; }}

        public int Rating { get; set; }

        public string Title { get; set; }

        public string Slug { get; set; }

        public ArticleImageViewModel MainArticlePic
        {
            get { return this._mainArticlePic; }
            set
            {
                if (value != null)
                {
                    this._mainArticlePic = value;
                }
            }
        }

        public string Category { get; set; }

        public DateTime CreatedOn { get; set; }

        public void CreateMappings(IMapperConfiguration configuration)
        {
            configuration.CreateMap<Article, AuthorArticleViewModel>()
                          .ForMember(x => x.Category, opt => opt.MapFrom(x => x.Category.Title))
                          .ForMember(x => x.Rating, opt => opt.MapFrom(x => x.Ratings.Any() ? (x.Ratings.Sum(y => y.Value) / x.Ratings.Count) : 0))
                          .ForMember(x => x.MainArticlePic, opt => opt.MapFrom(x => x.Images.FirstOrDefault(u => u.IsMain)));
        }
    }
}