namespace Uspelite.Web.Models.Home
{
    using System;
    using Infrastructure.Mapping.Contracts;
    using AutoMapper;
    using Data.Models;
    using System.Linq;

    public class ArticleViewModel : IMapFrom<Article>, IMapTo<Article>, IHaveCustomMappings
    {
        private int rating = 0;
        private int likesCount = 0;
        private int dislikesCount = 0;

        private ImageViewModel _mainArticlePic = new ImageViewModel
        {
            Url = "Content/Imgs/no-photo-available.jpg",
            IsMain = true,
            Title = "No photo available"
        };

        public int Id { get; set; }

        public string Title { get; set; }

        public ImageViewModel MainArticlePic
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
        
        public string Category { get; set; }

        public string PartialContent { get; set; }

        public int? Rating
        {
            get { return this.rating; }
            set
            {
                if (value != null)
                {
                    this.rating = (int)value;
                }

            }
        }

        public int? LikesCount
        {
            get { return this.likesCount; }
            set
            {
                if (value != null)
                {
                    this.likesCount = (int)value;
                }

            }
        }

        public int? DislikesCount
        {
            get { return this.dislikesCount; }
            set
            {
                if (value != null)
                {
                    this.dislikesCount = (int)value;
                }

            }
        }

        public void CreateMappings(IMapperConfiguration configuration)
        {
            configuration.CreateMap<Article, ArticleViewModel>()
           .ForMember(x => x.Rating, opt => opt.MapFrom(x => x.Ratings.Sum(y => y.Value) / x.Ratings.Count))
           .ForMember(x => x.LikesCount, opt => opt.MapFrom(x => x.Ratings.Count(y => y.IsPositive)))
           .ForMember(x => x.DislikesCount, opt => opt.MapFrom(x => x.Ratings.Count(y => y.IsPositive == false)))
           .ForMember(x => x.Category, opt => opt.MapFrom(x => x.Category.Title))
           .ForMember(x => x.PartialContent, opt => opt.MapFrom(x => x.Content.Substring(0, 45) + "..."))
           .ForMember(x => x.MainArticlePic, opt => opt.MapFrom(x => x.Images.FirstOrDefault(u => u.IsMain)));
        }
    }
}