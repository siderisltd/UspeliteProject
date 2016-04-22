namespace Uspelite.Services.Data.DTO
{
    using System.Collections.Generic;
    using Uspelite.Data.Models;

    public class CategoryAndPostsDTO
    {
        public Category Category { get; set; }

        public IEnumerable<Article> Posts { get; set; }
    }
}
