using System;
using System.Linq;
using Uspelite.Data.Models;

namespace DbMergeTool
{
    using System.Collections.Generic;

    internal class ArticlesMergeDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CategorySlug { get; set; }
        public string PictureInPostsId { get; set; }
        public string PostAuthorId { get; set; }
        public string PostContent { get; set; }
        public DateTime PostCreationDate { get; set; }
        public decimal PostId { get; set; }
        public string PostTitle { get; set; }
        public string AuthorEmail { get; set; }
        public string PostSlug { get; set; }
        public string PostTitleImageUrl { get; set; }
        public List<Comment> Comments { get; set; }
    }
}