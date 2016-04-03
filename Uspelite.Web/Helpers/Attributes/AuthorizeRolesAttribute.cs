namespace Uspelite.Web.Helpers.Attributes
{
    using System.Web.Mvc;

    public class AuthorizeRolesAttribute : AuthorizeAttribute
    {
        public AuthorizeRolesAttribute(params string[] roles)
        {
            this.Roles = string.Join(",", roles);
        }
    }
}