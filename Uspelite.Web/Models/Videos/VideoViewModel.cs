namespace Uspelite.Web.Models.Videos
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;
    using AutoMapper;
    using Common;
    using Data.Models;
    using Infrastructure.Mapping.Contracts;

    public class VideoViewModel : IRateable, IMapFrom<Video>, IHaveCustomMappings
    {
        private int rating = 0;

        public int Id { get; set; }

        public string Title { get; set; }

        public int RateType { get { return (int) RateableType.Video; } }


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


        public string VideoUrl { get; set; }

        public string EmbedUrl
        {
            get { return "http://www.youtube.com/embed/" + this.GetVideoId(this.VideoUrl); }
        }

        public string VideoImage
        {
            get
            {
                //Todo: add video images to the database to save youtube queryes
                return string.Format("http://img.youtube.com/vi/{0}/0.jpg", this.GetVideoId(this.VideoUrl));
            }
        }

        private string GetVideoId(string youtubeUrl)
        {
            //string pattern = @"v=([a-zA-Z0-9\_\-]+)&?";
            //var match = Regex.Match(youtubeUrl, pattern).Value;
            //var result = match.Substring(1, match.Length);

            string pattern = "/watch?v=";
            int index = youtubeUrl.IndexOf(pattern, StringComparison.Ordinal);
            var length = youtubeUrl.IndexOf('&', index + pattern.Length);
            string videoId;
            int startIdIndex = index + pattern.Length;
            if (length < 0)
            {
                videoId = youtubeUrl.Substring(startIdIndex);
            }
            else
            {
                videoId = youtubeUrl.Substring(startIdIndex, length - startIdIndex);
            }


            return videoId;
        }

        public void CreateMappings(IMapperConfiguration configuration)
        {
            configuration.CreateMap<Video, VideoViewModel>()
                         .ForMember(x => x.Rating, opt => opt.MapFrom(x => x.Ratings.DefaultIfEmpty(new Rate {Value= 0 }).Sum(y => y.Value) / x.Ratings.DefaultIfEmpty(new Rate()).Count()));
        }
    }
}