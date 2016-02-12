namespace Uspelite.Web.Models
{
    using System;
    using System.Linq.Expressions;
    using Data.Models;
    using Infrastructure.Mapping.Contracts;

    public class PictureViewModel : IMapFrom<Picture>, IMapTo<Picture>
    {
        public static Expression<Func<Picture, PictureViewModel>> FromModel
        {
            get
            { 
                return x => new PictureViewModel
                    {
                        IsMainPicture = x.IsMainPicture, 
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