namespace Uspelite.Web.Models
{
    using System;
    using System.Linq.Expressions;
    using Data.Models;
    using Infrastructure.Mapping.Contracts;

    public class ImageViewModel : IMapFrom<Image>, IMapTo<Image>
    {
        public static Expression<Func<Image, ImageViewModel>> FromModel
        {
            get
            { 
                return x => new ImageViewModel
                    {
                        IsMainPicture = x.IsMain, 
                        Title = x.Title,
                        Url = x.Url
                    };
            }
        }

        public string Title { get; set; }

        public string Url { get; set; }

        public bool IsMainPicture { get; set; }
    }
}