namespace Uspelite.Web.Models.Home
{
    using System;
    using Infrastructure.Mapping.Contracts;
    using AutoMapper;
    using Data.Models;
    using System.Linq;

    public class PostViewModel : IMapFrom<Post>, IMapTo<Post>, IHaveCustomMappings
    {
        private int rating = 0;
        private int likesCount = 0;
        private int dislikesCount = 0;

        private PictureViewModel mainPicture = new PictureViewModel
        {
            Url = "Content/Imgs/no-photo-available.jpg",
            IsMainPicture = true,
            Title = "No photo available"
        };

        public int Id { get; set; }

        public string Title { get; set; }

        public PictureViewModel MainPicture
        {
            get { return this.mainPicture; }
            set
            {
                if (value != null)
                {
                    this.mainPicture = value;
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
            configuration.CreateMap<Post, PostViewModel>()
           .ForMember(x => x.Rating, opt => opt.MapFrom(x => x.Rates.Sum(y => y.Value) / x.Rates.Count))
           .ForMember(x => x.LikesCount, opt => opt.MapFrom(x => x.Rates.Count(y => y.IsPositive)))
           .ForMember(x => x.DislikesCount, opt => opt.MapFrom(x => x.Rates.Count(y => y.IsPositive == false)))
           .ForMember(x => x.MainPicture, opt => opt.MapFrom(x => x.Pictures.Where(y => y.IsMainPicture).Select(z => new PictureViewModel { Title = z.Title, IsMainPicture = z.IsMainPicture, Url = z.Url }).FirstOrDefault()))
           .ForMember(x => x.Category, opt => opt.MapFrom(x => x.Categories.FirstOrDefault().Title))
           .ForMember(x => x.PartialContent, opt => opt.MapFrom(x => x.Content.Substring(0, 45) + "..."));
        }
    }
}