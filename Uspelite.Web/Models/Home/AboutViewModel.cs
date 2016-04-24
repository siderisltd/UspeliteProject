namespace Uspelite.Web.Models.Home
{
    using System.Collections.Generic;
    using Areas.Administration.Models.Users;
    using Authors;

    public class AboutViewModel
    {
        public IList<AuthorsUserViewModel> Users { get; set; }
    }
}