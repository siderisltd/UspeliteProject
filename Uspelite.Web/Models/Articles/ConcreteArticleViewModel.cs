namespace Uspelite.Web.Models.Articles
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using Categories;
    using Comments;
    using Common;
    using Data.Models;
    using Infrastructure.Mapping.Contracts;
    using Services.Web;
    using Services.Web.Contracts;

    public class ConcreteArticleViewModel : IRateable, IMapFrom<Article>, IMapTo<Article>, IHaveCustomMappings
    {
        protected readonly ISanitizer sanitizer;

        public ConcreteArticleViewModel()
            : this(new HtmlSanitizerAdapter())
        {

        }

        public ConcreteArticleViewModel(ISanitizer sanitizer)
        {
            this.sanitizer = sanitizer;
        }

        private int rating = 0;

        private ArticleImageViewModel _mainArticlePic = new ArticleImageViewModel
        {
            Resized400Picture = "Content/Imgs/no-photo-available.jpg",
            IsMain = true,
            Title = "No photo available"
        };

        public int Id { get; set; }

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

        public DateTime CreatedOn { get; set; }

        public CategoryInfoViewModel Category { get; set; }

        public string PartialContent { get; set; }

        public string FullContent { get; set; }

        public string SanitizedFullContent
        {
            get { return this.sanitizer.Sanitize(this.FullContent); }
        }

        public int RateType {  get { return (int) RateableType.Article; }  }

        public int Rating { get; set; }

        public UserViewModel Author { get; set; }

        public ICollection<CommentViewModel> Comments { get; set; }

        public bool ShowEdit { get; set; }

        public IList<ArticleViewModel> RelatedArticles { get; set; }

        public virtual void CreateMappings(IMapperConfiguration configuration)
        {
            configuration.CreateMap<Article, ConcreteArticleViewModel>()
                .ForMember(x => x.Rating, opt => opt.MapFrom(x => x.Ratings.Any() ? (x.Ratings.Sum(y => y.Value) / x.Ratings.Count) : 0))
                .ForMember(x => x.FullContent, opt => opt.MapFrom(x => x.Content))
                .ForMember(x => x.MainArticlePic, opt => opt.MapFrom(x => x.Images.FirstOrDefault(u => u.IsMain)));
        }
    }

}