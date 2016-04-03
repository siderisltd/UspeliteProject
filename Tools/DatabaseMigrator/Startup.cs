namespace DatabaseMigrator
{
    using System;
    using System.Linq;
    using System.Collections;
    using System.Collections.Generic;
    using Uspelite.Data;

    public class Startup
    {
        static void Main()
        {
            var oldData = new uspelite_oldDbConnectionString();
            var newData = new UspeliteDbContext();

            try
            {
                var oldUsers = oldData.wp_users.ToList();
                foreach (var user in oldUsers)
                {
                    Console.WriteLine(user.user_login);
                }

                var videos = newData.Videos.ToList();
                foreach (var vid in videos)
                {
                    Console.WriteLine(vid.Id);
                }
            }

            finally
            {
                oldData.Dispose();
            }

        }
    }
}
