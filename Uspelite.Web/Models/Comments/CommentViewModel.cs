namespace Uspelite.Web.Models.Comments
{
    using System;
    using System.Collections.Generic;
    using Common;
    using Data.Models;
    using Infrastructure.Mapping.Contracts;
    using Services.Web;
    using Services.Web.Contracts;

    public class CommentViewModel : IMapFrom<Comment>
    {
        protected readonly ISanitizer sanitizer;

        public CommentViewModel()
            :this(new HtmlSanitizerAdapter())
        {
        }

        public CommentViewModel(ISanitizer sanitizer)
        {
            this.sanitizer = sanitizer;
        }

        public int Id { get; set; }

        public string Content { get; set; }

        public string SanitizedContent
        {
            get { return this.sanitizer.Sanitize(this.Content); }
        }

        public UserViewModel Author { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}