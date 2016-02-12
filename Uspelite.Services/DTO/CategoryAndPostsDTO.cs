namespace Uspelite.Services.Data.DTO
{
    using System.Collections.Generic;
    using Uspelite.Data.Models;

    public class CategoryAndPostsDTO
    {
        public string CategoryName { get; set; }

        public IEnumerable<Post> Posts { get; set; }
    }
}
