﻿namespace Uspelite.Web.Models.ChildActions
{
    using System.Collections.Generic;
    using Home;
    using Infrastructure.Mapping.Contracts;
    using Services.Data.DTO;

    public class CategoryAndPostsViewModel : IMapFrom<CategoryAndPostsDTO>
    {
        public string CategoryName { get; set; }

        public IEnumerable<PostViewModel> Posts { get; set; }
    }
}