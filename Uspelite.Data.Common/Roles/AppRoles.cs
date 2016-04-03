namespace Uspelite.Data.Common.Roles
{
    using System.Collections.Generic;

    public class AppRoles
    {
        public const string ULTIMATE_ROLE = "Ultimate";
        public const string ADMIN_ROLE = "Administrator";
        public const string MANAGER_ROLE = "Manager";
        public const string EDITOR_ROLE = "Editor";
        public const string CLIENT_ROLE = "Client";

        public static readonly HashSet<string> AllRoles = new HashSet<string>()
        {
            ULTIMATE_ROLE,
            ADMIN_ROLE,
            MANAGER_ROLE,
            EDITOR_ROLE,
            CLIENT_ROLE
        };
    }
}
