namespace Uspelite.Services.Data.DTO
{
    using System.Collections.Generic;
    using System.Linq;
    using NinjaNye.SearchExtensions.Models;
    using Uspelite.Data.Models;

    public class SearchArticleResultsDTO
    {
        public IOrderedQueryable<IRanked<Article>> ResultsInTitles { get; set; }

        public IOrderedQueryable<IRanked<Article>> ResultsInContents { get; set; }

        public int AllSearchedResultsCount => this.ResultsInTitles.Count() + this.ResultsInContents.Count();
    }
}