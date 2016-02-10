﻿namespace Uspelite.Services.Data
{
    using Contracts;
    using Uspelite.Data.Models;
    using Uspelite.Data.Repositories;

    public class CategoriesService : ICategoriesService
    {
        private readonly IRepository<Category> repo;

        public CategoriesService(IRepository<Category> repo)
        {
            this.repo = repo;
        }
    }
}