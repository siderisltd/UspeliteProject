﻿using System.Linq;
using Uspelite.Data.Models;

namespace Uspelite.Services.Data.Contracts
{
    public interface ICategoriesService
    {
        IQueryable<Category> GetAllCategories();
    }
}
