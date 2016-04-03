namespace Uspelite.Data.Models
{
    using System.Collections.Generic;
    using BaseModels;

    public class Articles_Videos : BaseModel
    {
        public int Id { get; set; }

        public int ArticleId { get; set; }

        public Article Article { get; set; }

        public int VideoId { get; set; }

        public Video Video { get; set; }

    }
}
