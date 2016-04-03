namespace Uspelite.Web.Models.Comments
{
    using System.ComponentModel.DataAnnotations;
    using Common;

    public class CommentsBindingModel
    {
        public int toId { get; set; }

        [MaxLength(1000)]
        public string Content { get; set; }
    }
}