﻿namespace Uspelite.Web.Models.Articles
{
    using System;
    using System.Linq;
    using AutoMapper;
    using Common;
    using Data.Models;
    using Infrastructure.Mapping.Contracts;
    using Services.Web;
    using Services.Web.Contracts;

    public class ArticleViewModel : IRateable, IMapFrom<Article>, IMapTo<Article>, IHaveCustomMappings 
    {
        protected readonly ISanitizer sanitizer;

        public ArticleViewModel()
            :this(new HtmlSanitizerAdapter())
        {

        }

        public ArticleViewModel(ISanitizer sanitizer)
        {
            this.sanitizer = sanitizer;
        }

        private int rating = 0;
        private int likesCount = 0;
        private int dislikesCount = 0;

        private ImageViewModel _mainArticlePic = new ImageViewModel
        {
            Resized400Picture = "Content/Imgs/no-photo-available.jpg",
            IsMain = true,
            Title = "No photo available"
        };

        public int Id { get; set; }

        public string Title { get; set; }

        public string Slug { get; set; }

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

        public int RateType { get { return (int)RateableType.Article; } }

        public string SanitizedPartialContent
        {
            get { return this.sanitizer.Sanitize(this.PartialContent); }
        }

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

        public virtual void CreateMappings(IMapperConfiguration configuration)
        {
            configuration.CreateMap<Article, ArticleViewModel>()
           .ForMember(x => x.Rating, opt => opt.MapFrom(x => x.Ratings.Sum(y => y.Value) / x.Ratings.Count))
           .ForMember(x => x.LikesCount, opt => opt.MapFrom(x => x.Ratings.Count(y => y.IsPositive)))
           .ForMember(x => x.DislikesCount, opt => opt.MapFrom(x => x.Ratings.Count(y => y.IsPositive == false)))
           .ForMember(x => x.Category, opt => opt.MapFrom(x => x.Category.Slug))
           .ForMember(x => x.PartialContent, opt => opt.MapFrom(x => x.Content.Substring(0, 45) + "..."))
           .ForMember(x => x.MainArticlePic, opt => opt.MapFrom(x => x.Images.FirstOrDefault(u => u.IsMain)));
        }
    }
}