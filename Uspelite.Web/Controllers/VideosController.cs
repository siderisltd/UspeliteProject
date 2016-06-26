namespace Uspelite.Web.Controllers
{
    using System;
    using System.Web.Mvc;
    using Models.Videos;
    using Services.Data.Contracts;
    using Data.Models;
    using System.Net.Http;    //using Microsoft.AspNet.Identity;
    using Newtonsoft.Json;
    using Microsoft.AspNet.Identity;
    using Infrastructure;
    using Newtonsoft.Json.Linq;
    using System.Text.RegularExpressions;
    public class VideosController : BaseController
    {
        private const int PAGE_SIZE = 15;
        private readonly IVideosService videosService;

        public VideosController(IVideosService videosService)
        {
            this.videosService = videosService;
        }

        [HttpGet]
        public ActionResult Index(int page = 1, int pageSize = PAGE_SIZE)
        {
            var dto = this.videosService.AllPaged(page, pageSize);
            var model = this.Mapper.Map<PageableVideoViewModel>(dto);
            model.PageSize = pageSize;

            if (page <= 6)
            {
                model.DisplayPageFrom = 1;
                model.DisplayPageTo = 10 > model.TotalPages ? model.TotalPages : 10;
            }
            else if (page > 6)
            {
                var displayTo = page + 4;
                model.DisplayPageTo = model.TotalPages <= displayTo ? model.TotalPages : displayTo;
                model.DisplayPageFrom = model.DisplayPageTo - 9;
            }

            return this.View(model);
        }

        [HttpGet]
        public string GetEmbedLink(string videoUrl)
        {
            var model = new VideoViewModel(videoUrl);

            //var videoInfoUrl = "http://youtube.com/get_video_info?video_id=QXzPbN001Cw";
         

            //var webRequester = new WebRequester();
            //var result = webRequester.Request(videoInfoUrl, Infrastructure.Enums.MethodType.Get).Result;
            //var jsonHelper = new JsonHelper();

            //if (!string.IsNullOrEmpty(result))
            //{
            //    dynamic videoInfo = jsonHelper.NestedDeserializeDynamicDictionary(result);

            //    if (videoInfo == null)
            //    {
            //        return "YOU CAN'T INSERT NON YOUTUBE VIDEO";
            //    }
            //    else
            //    {
            //        dynamic title = videoInfo["title"];
            //        videoTitle = videoInfo.Title;
            //    }
            //}
            var youtubeRegexPattern = @"(youtu\.be\/|youtube\.com\/(watch\?(.*&)?v=|(embed|v)\/))([^\?&""'>]+)";
            if (!Regex.IsMatch(videoUrl, youtubeRegexPattern))
            {
                throw new ArgumentException("Only youtube videos are allowed");
            }
            //TODO: Video category is currently default one and video title is the url
            string videoTitle = videoUrl;

            var videoAsDbModel = new Video { AuthorId = this.User.Identity.GetUserId(), VideoUrl = videoUrl, Title = videoTitle, CategoryId = 5 };
            this.videosService.Add(videoAsDbModel);
            this.videosService.SaveChanges();

            return model.EmbedUrl;
        }
    }
}