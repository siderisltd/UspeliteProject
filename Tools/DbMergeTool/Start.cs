namespace DbMergeTool
{
    using System;
    using System.Collections.Generic;
    using System.Drawing.Imaging;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using Uspelite.Data;
    using Uspelite.Data.Models;
    using Uspelite.Data.Repositories;
    using Uspelite.Services.Common;
    using Uspelite.Services.Data;
    using Uspelite.Web.Models.Videos;

    public class Start
    {
        static void Main()
        {
            UspeliteDbContext ctx = new UspeliteDbContext();
            //var mergeWorker = new MergeWorker();
            //mergeWorker.Start();

            //var users = ctx.Users.ToList();
            var allArticles = ctx.Articles.ToList();
            var uniqueTags = GetUniqueBracketTags(ctx);



            foreach (var article in allArticles)
            {
                var text = article.Content;

                string patternFirst = @"\[(quote+.*?)\]";
                string patternSecond = @"\[(\/quote+.*?)]";

                text = Regex.Replace(text, patternFirst, "<blockquote>", RegexOptions.IgnoreCase);
                text = Regex.Replace(text, patternSecond, "</blockquote>", RegexOptions.IgnoreCase);


                string pattFir = @"\[(caption+.*?)\]";
                string pattSec = @"\[(\/caption+.*?)]";

                text = Regex.Replace(text, pattFir, "<div>", RegexOptions.IgnoreCase);
                text = Regex.Replace(text, pattSec, "</div>", RegexOptions.IgnoreCase);


                var pattern = @"\[(.*?)\]";
                text = Regex.Replace(text, pattern, string.Empty);

                article.Content = text;
               

                ctx.SaveChanges();
            }

            //ReplaceCaption(allArticles, ctx);
            //ReplaceQuote(allArticles, ctx);
            //ReplaceZero(allArticles, ctx);
            //ReplaceHighlight(allArticles, ctx);

            //AddParagraphsToEmptyRows(allArticles, ctx);
            ////AddDefaultUserPictureAndDescIfNone(users, ctx);

            //ReplaceVideos(allArticles, ctx);
        }

        //private static void RemoveSquareBrackets(List<Article> allArticles, UspeliteDbContext ctx)
        //{
        //    foreach (var article in allArticles)
        //    {


        //        var content = article.Content;

        //        var startIndex = 0;




        //        List<string> tags = new List<string>();
        //        var pattern = @"\[(.*?)\]";


        //        content = Regex.Replace(content, pattern, string.Empty);


        //        article.Content = content;
        //        //ctx.SaveChanges();
        //    }
        //}



        private static void ReplaceVideos(List<Article> allArticles, UspeliteDbContext ctx)
        {
            foreach (var article in allArticles)
            {
                var content = article.Content;

                var startIndex = 0;


                var openBracketIndex = content.IndexOf("[embed]", startIndex, StringComparison.Ordinal);

                var count = 0;
                while (openBracketIndex > 0)
                {
                    var firstIndexOfCloseBracket = (content.IndexOf(']', openBracketIndex));

                    var endOfVideoUrl = content.IndexOf("[/embed]", firstIndexOfCloseBracket, StringComparison.Ordinal);

                    var videoUrl = content.Substring(firstIndexOfCloseBracket + 1, endOfVideoUrl - openBracketIndex - 7);


                    var video = new Video
                    {
                        AuthorId = article.AuthorId,
                        VideoUrl = videoUrl,
                        Title = article.Title + "-video-" + count.ToString(),
                        Category = article.Category
                    };

                    ctx.Videos.Add(video);
                    ctx.Article_Videos.Add(new Articles_Videos { ArticleId = article.Id, VideoId = video.Id });

                    VideoViewModel model = new VideoViewModel(videoUrl);

                    var embedLink = model.EmbedUrl;

                    var htmlEmbedLink = @"<div class=""video-container"" id=""video-container"">
    <iframe width=""560"" height=""315"" src="" " + embedLink + "\" frameborder=\"0\" allowfullscreen></iframe></div>";

                    var contentLen = content.Length;
                    var replacedContent = content.Remove(openBracketIndex, endOfVideoUrl - openBracketIndex + 8);
                    var insertedWIthEmbedLink = replacedContent.Insert(openBracketIndex, htmlEmbedLink);


                    article.Content = insertedWIthEmbedLink;
                    startIndex = openBracketIndex + 10;
                    content = insertedWIthEmbedLink;

                    count++;

                    var art = ctx.Articles.FirstOrDefault(x => x.Id == article.Id);
                    art.Content = insertedWIthEmbedLink;
                    ctx.SaveChanges();

                    openBracketIndex = content.IndexOf("[embed]", startIndex, StringComparison.Ordinal);
                }
            }
        }

        private static void AddDefaultUserPictureAndDescIfNone(List<User> users, UspeliteDbContext ctx)
        {
            foreach (var user in users)
            {
                var imageUrl = "http://princeps.bg/blog/wp-content/uploads/2010/03/no_image.gif";
                var imageTitle = Guid.NewGuid().ToString();
                var userId = "f506e580-1055-402f-9bce-b8881102af7b";

                ImagesService imagesService = new ImagesService(new GenericRepository<Image>(ctx), new ImageHelper());
                var img = imagesService.SaveImageFromWeb(imageUrl, imageTitle, ImageFormat.Jpeg, userId);
                img.IsMainProfilePicture = true;


                if (user.ProfileImages.Count == 0)
                {
                    user.ProfileImages.Add(img);
                }
                ctx.SaveChanges();
            }
        }

        private static void AddParagraphsToEmptyRows(List<Article> allContents, UspeliteDbContext ctx)
        {
            for (int i = 0; i < allContents.Count; i++)
            {
                var cur = allContents[i].Content;
                var startP = "<p>";
                var endP = "</p>";
                var current = "";
                current += startP;
                current += cur;

                var startIndex = 0;
                var len = current.Length;
                while (current.IndexOf("\r\n", startIndex) > 0)
                {
                    var index = current.IndexOf("\r\n", startIndex);
                    current = current.Insert(index, startP);
                    current = current.Insert(index, endP);

                    startIndex += index + 9;
                    if (startIndex >= len)
                    {
                        break;
                    }
                }


                current += endP;
                allContents[i].Content = current;

                if (i % 100 == 0)
                {
                    ctx.SaveChanges();
                }
            }
            ctx.SaveChanges();
        }

        private static IEnumerable<string> GetUniqueBracketTags(UspeliteDbContext ctx)
        {
            var allArticles = ctx.Articles
             .Where(x => x.Content.Contains("["))
             .Select(x => x.Content)
             .ToList();

            List<string> tags = new List<string>();
            var pattern = @"\[(.*?)\]";

            foreach (var content in allArticles)
            {
                var match = Regex.Match(content, pattern);

                while (match.Success)
                {
                    tags.Add(match.Value);
                    match = match.NextMatch();
                }

            }
            var uniqueTags = tags.Select(x => x).Distinct();

            return uniqueTags;
        }
    }
}
