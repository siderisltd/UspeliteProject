namespace Uspelite.Data.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Common;
    using Common.Contracts;
    using Common.Roles;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using Models.Enum;

    public sealed class Configuration : DbMigrationsConfiguration<Uspelite.Data.UspeliteDbContext>
    {
        private IRandomGenerator randomGenerator;

        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
            this.randomGenerator = new RandomGenerator();
        }

        protected override void Seed(UspeliteDbContext context)
        {
            var seedCondition = context.Roles.Count() < 3 && context.Users.Count() < 5 && context.Categories.Count() < 5;

            if (seedCondition)
            {
                var userManager = new UserManager<User>(new UserStore<User>(context));
                this.SeedAppRoles(context);
                IList<User> seededUsers = this.SeedAppUsers(context, userManager);


                var seededCategories = this.SeedCategories(context, 30);

                var allVideoUrls = this.GetAllVideoUrls();
                var seededVideos = this.SeedVideos(context, seededUsers, allVideoUrls, seededCategories);

                context.SaveChanges();

                var allPictures = this.GetAllPictures(30, seededUsers);
                this.SeedPictures(context, seededUsers, allPictures);
                IList<Post> seededPosts = this.SeedPosts(context, seededUsers, seededCategories, allPictures);

                context.SaveChanges();



                this.SeedComments(context, seededUsers, seededPosts, seededVideos);
                this.SeedRates(context, seededUsers, seededPosts, seededVideos);
                try
                {

                    context.SaveChanges();
                }
                catch (Exception)
                {
                    
                    throw;
                }
            }

        }

        private void SeedRates(UspeliteDbContext context, IList<User> seededUsers, IList<Post> seededPosts, IList<Video> seededVideos)
        {
            foreach (var post in seededPosts)
            {
                var ratesCount = this.randomGenerator.RandomIntegerBetween(0, 10);
                for (int i = 0; i < ratesCount; i++)
                {
                    var randomUser = seededUsers[this.randomGenerator.RandomIntegerBetween(0, seededUsers.Count - 1)];
                    var randomNumber = this.randomGenerator.RandomIntegerBetween(1, 5);
                    var rateToAdd = new Rate();
                    if (randomNumber % 2 == 0)
                    {
                        //positive rate
                        rateToAdd.IsPositive = true;
                        rateToAdd.Value = randomNumber;
                        rateToAdd.Author = randomUser;
                    }
                    else
                    {
                        //negative rate
                        rateToAdd.IsPositive = false;
                        rateToAdd.Value = randomNumber;
                        rateToAdd.Author = randomUser;
                    }

                    post.Rates.Add(rateToAdd);
                }
            }

            foreach (var video in seededVideos)
            {
                var ratesCount = this.randomGenerator.RandomIntegerBetween(0, 10);
                for (int i = 0; i < ratesCount; i++)
                {
                    var randomUser = seededUsers[this.randomGenerator.RandomIntegerBetween(0, seededUsers.Count - 1)];
                    var randomNumber = this.randomGenerator.RandomIntegerBetween(1, 5);
                    var rateToAdd = new Rate();
                    if (randomNumber % 2 == 0)
                    {
                        //positive rate
                        rateToAdd.IsPositive = true;
                        rateToAdd.Value = randomNumber;
                        rateToAdd.Author = randomUser;
                    }
                    else
                    {
                        //negative rate
                        rateToAdd.IsPositive = false;
                        rateToAdd.Value = randomNumber;
                        rateToAdd.Author = randomUser;
                    }

                    video.Rates.Add(rateToAdd);
                }
            }
        }

        private void SeedComments(UspeliteDbContext context, IList<User> seededUsers, IList<Post> seededPosts, IList<Video> seededVideos)
        {
            foreach (var post in seededPosts)
            {
                var postsCount = this.randomGenerator.RandomIntegerBetween(0, 20);
                for (int i = 0; i < postsCount; i++)
                {
                    var randomUser = seededUsers[this.randomGenerator.RandomIntegerBetween(0, seededUsers.Count - 1)];

                    var commentToAdd = new Comment()
                    {
                        Author = randomUser,
                        Content = this.randomGenerator.RandomText(20, 5, 10)
                    };

                    post.Comments.Add(commentToAdd);
                }
            }

            foreach (var video in seededVideos)
            {
                var postsCount = this.randomGenerator.RandomIntegerBetween(0, 200);
                for (int i = 0; i < postsCount; i++)
                {
                    var randomUser = seededUsers[this.randomGenerator.RandomIntegerBetween(0, seededUsers.Count - 1)];

                    var commentToAdd = new Comment()
                    {
                        Author = randomUser,
                        Content = this.randomGenerator.RandomText(20, 5, 10)
                    };

                    video.Comments.Add(commentToAdd);
                }
            }
        }

        private Picture[] GetAllPictures(int count, IList<User> allUsers)
        {


            IList<Picture> result = new List<Picture>();

            for (int i = 0; i < count; i++)
            {
                var randomUser = allUsers[this.randomGenerator.RandomIntegerBetween(0, allUsers.Count - 1)];
                var picture = new Picture
                {
                    AltTag = this.randomGenerator.RandomString(),
                    Title = "PictureTitle" + i,
                    ImageType = ImageType.Jpeg,
                    Author = randomUser,
                    Url = "/1/testImage.jpg",
                };

                result.Add(picture);
            }

            return result.ToArray();
        }

        private void SeedPictures(UspeliteDbContext context, IList<User> allUsers, Picture[] allPictures)
        {
            for (int i = 0; i < 30; i++)
            {
                context.Pictures.AddOrUpdate(x => x.Title, allPictures);
            }
        }

        private IList<Post> SeedPosts(UspeliteDbContext context, IList<User> allUsers, IList<Category> allCategories, Picture[] allPictures)
        {
            var maxPostsPerUser = this.randomGenerator.RandomIntegerBetween(0, 30);

            IList<Post> postsToAdd = new List<Post>();

            foreach (var user in allUsers)
            {
                for (int i = 0; i < maxPostsPerUser; i++)
                {
                    var randomCategories = new HashSet<Category>();
                    var randomCategoriesCount = this.randomGenerator.RandomIntegerBetween(1, 4);
                    for (int j = 0; j < randomCategoriesCount; j++)
                    {
                        randomCategories.Add(allCategories[this.randomGenerator.RandomIntegerBetween(0, allCategories.Count - 1)]);
                    }

                    var wordsInPost = i % 2 == 0 ? 2 : 3;
                    var post = new Post
                    {
                        Author = user,
                        Title = this.randomGenerator.RandomText(wordsInPost) + i,
                        Content = this.randomGenerator.RandomText(50, 2, 12),
                        Status = Models.Enum.PostStatus.Published,
                        Categories = randomCategories,

                    };

                    postsToAdd.Add(post);
                }
            }

            context.Posts.AddOrUpdate(x => x.Title, postsToAdd.ToArray());

            return postsToAdd;
        }

        private IList<Category> SeedCategories(UspeliteDbContext context, int count)
        {
            IList<Category> result = new List<Category>();
            for (int i = 0; i < count; i++)
            {
                var category = new Category() { Title = "Test" + i };
                context.Categories.AddOrUpdate(x => x.Title, category);
                result.Add(category);
            }
            return result;
        }

        private IList<Video> SeedVideos(UspeliteDbContext context, IList<User> allUsers, IList<string> allVideoUrls, IList<Category> allCategories)
        {
            if (allUsers.Count == 0)
            {
                return new List<Video>();
            }

            IList<Video> result = new List<Video>();

            var videosCountPerUser = this.randomGenerator.RandomIntegerBetween(0, allUsers.Count - 1);
            for (int i = 0; i < videosCountPerUser; i++)
            {
                var randomCategories = new HashSet<Category>();
                var randomCategoriesCount = this.randomGenerator.RandomIntegerBetween(1, 4);
                for (int j = 0; j < randomCategoriesCount; j++)
                {
                    randomCategories.Add(allCategories[this.randomGenerator.RandomIntegerBetween(0, allCategories.Count - 1)]);
                }
                var currentVideo = new Video
                {
                    Author = allUsers[i],
                    Title = this.randomGenerator.RandomText(2, 4, 7) + i,
                    VideoUrl = allVideoUrls[this.randomGenerator.RandomIntegerBetween(0, allVideoUrls.Count - 1)],
                    Categories = randomCategories
                };

                context.Videos.AddOrUpdate(x => x.Title, currentVideo);
                result.Add(currentVideo);
            }

            return result;
        }

        private IList<User> SeedAppUsers(UspeliteDbContext context, UserManager<User> userManager)
        {
            var result = new List<User>();
            for (int i = 0; i < 30; i++)
            {
                var currentUser = new User
                {
                    UserName = "Testov" + i,
                    Email = string.Format("{0}@{1}.{2}", this.randomGenerator.RandomString(4, 7), this.randomGenerator.RandomString(2, 4), this.randomGenerator.RandomString(2, 4))
                };

                if (!userManager.Users.Any(x => x.UserName == currentUser.UserName))
                {
                    userManager.Create(currentUser, "Pass123");
                    userManager.AddToRole(currentUser.Id, AppRoles.CLIENT_ROLE);
                    result.Add(currentUser);
                }

            }

            var ultimateUsername = "ultimate";
            var ultimateUser = new User { UserName = ultimateUsername };

            if (!userManager.Users.Any(x => x.UserName == ultimateUsername))
            {
                userManager.Create(ultimateUser, "ultimate");
                userManager.AddToRole(ultimateUser.Id, AppRoles.ULTIMATE_ROLE);
                result.Add(ultimateUser);
            }

            return result;
        }

        private void SeedAppRoles(UspeliteDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            foreach (var roleName in AppRoles.AllRoles)
            {
                if (!roleManager.RoleExists(roleName))
                {
                    context.Roles.Add(new IdentityRole(roleName));
                }
            }
        }

        private IList<string> GetAllVideoUrls()
        {
            return new List<string>
                   {
                        "https://www.youtube.com/watch?v=DWaB4PXCwFU&list=PLA03jGxzdSLCiDnyRptnV9E8tt78yO-sT",
                        "https://www.youtube.com/watch?v=d8ekz_CSBVg&index=2&list=PLA03jGxzdSLCiDnyRptnV9E8tt78yO-sT",
                        "https://www.youtube.com/watch?v=ysSxxIqKNN0&index=3&list=PLA03jGxzdSLCiDnyRptnV9E8tt78yO-sT",
                        "https://www.youtube.com/watch?v=R_HHm9ki3JI&list=PLA03jGxzdSLCiDnyRptnV9E8tt78yO-sT&index=4",
                        "https://www.youtube.com/watch?v=lL2ZwXj1tXM&list=PLA03jGxzdSLCiDnyRptnV9E8tt78yO-sT&index=6",
                        "https://www.youtube.com/watch?v=kXYiU_JCYtU&index=7&list=PLA03jGxzdSLCiDnyRptnV9E8tt78yO-sT",
                        "https://www.youtube.com/watch?v=kXYiU_JCYtU&index=7&list=PLA03jGxzdSLCiDnyRptnV9E8tt78yO-sT",
                        "https://www.youtube.com/watch?v=7qrRzNidzIc&list=PLA03jGxzdSLCiDnyRptnV9E8tt78yO-sT&index=8",
                        "https://www.youtube.com/watch?v=xqds0B_meys&index=9&list=PLA03jGxzdSLCiDnyRptnV9E8tt78yO-sT",
                        "https://www.youtube.com/watch?v=8sgycukafqQ&index=10&list=PLA03jGxzdSLCiDnyRptnV9E8tt78yO-sT",
                        "https://www.youtube.com/watch?v=pdoIs1jZbCY&index=11&list=PLA03jGxzdSLCiDnyRptnV9E8tt78yO-sT",
                        "https://www.youtube.com/watch?v=KGrM1sh-8pE&list=PLA03jGxzdSLCiDnyRptnV9E8tt78yO-sT&index=12",
                        "https://www.youtube.com/watch?v=Ud4HuAzHEUc&index=13&list=PLA03jGxzdSLCiDnyRptnV9E8tt78yO-sT",
                        "https://www.youtube.com/watch?v=1yw1Tgj9-VU&index=14&list=PLA03jGxzdSLCiDnyRptnV9E8tt78yO-sT",
                        "https://www.youtube.com/watch?v=_4VCpTZye10&list=PLA03jGxzdSLCiDnyRptnV9E8tt78yO-sT&index=17",
                        "https://www.youtube.com/watch?v=Gd9OhYroLN0&index=18&list=PLA03jGxzdSLCiDnyRptnV9E8tt78yO-sT",
                        "https://www.youtube.com/watch?v=d-fx-hptjUg&list=PLA03jGxzdSLCiDnyRptnV9E8tt78yO-sT&index=19",
                        "https://www.youtube.com/watch?v=04fQTmvFfGo&list=PLA03jGxzdSLCiDnyRptnV9E8tt78yO-sT&index=21",
                        "https://www.youtube.com/watch?v=51iquRYKPbs&index=22&list=PLA03jGxzdSLCiDnyRptnV9E8tt78yO-sT",
                        "https://www.youtube.com/watch?v=ocXjr9nPnvg&index=23&list=PLA03jGxzdSLCiDnyRptnV9E8tt78yO-sT",
                        "https://www.youtube.com/watch?v=9gpkOIwCyI8&list=RD9gpkOIwCyI8#t=2",
                        "https://www.youtube.com/watch?v=EkHTsc9PU2A&list=RD9gpkOIwCyI8&index=2",
                        "https://www.youtube.com/watch?v=bcQwIxRcaYs&list=RD9gpkOIwCyI8&index=3",
                        "https://www.youtube.com/watch?v=O1-4u9W-bns&list=RD9gpkOIwCyI8&index=4",
                        "https://www.youtube.com/watch?v=gMkWxjVv-hU&index=5&list=RD9gpkOIwCyI8",
                        "https://www.youtube.com/watch?v=zkbTp3-zBGg&index=6&list=RD9gpkOIwCyI8",
                        "https://www.youtube.com/watch?v=acvIVA9-FMQ&index=7&list=RD9gpkOIwCyI8",
                        "https://www.youtube.com/watch?v=LKJZQ7dbu14&index=8&list=RD9gpkOIwCyI8",
                        "https://www.youtube.com/watch?v=e2wIq8qggzs&index=9&list=RD9gpkOIwCyI8",
                        "https://www.youtube.com/watch?v=qUCkkpYcn_g&index=10&list=RD9gpkOIwCyI8",
                        "https://www.youtube.com/watch?v=WTV1hFM3XpU&index=11&list=RD9gpkOIwCyI8",
                        "https://www.youtube.com/watch?v=d85RuIhs5Ww&index=12&list=RD9gpkOIwCyI8",
                        "https://www.youtube.com/watch?v=H600M2_juM4&index=13&list=RD9gpkOIwCyI8",
                        "https://www.youtube.com/watch?v=OsrICJqWQ1E&index=14&list=RD9gpkOIwCyI8",
                        "https://www.youtube.com/watch?v=SdryssUmlpE&index=15&list=RD9gpkOIwCyI8",
                        "https://www.youtube.com/watch?v=_r2CwihdLkc&index=16&list=RD9gpkOIwCyI8",
                        "https://www.youtube.com/watch?v=YUFs_1vKYlY&index=17&list=RD9gpkOIwCyI8",
                        "https://www.youtube.com/watch?v=oieBnV_HFB0&index=18&list=RD9gpkOIwCyI8",
                        "https://www.youtube.com/watch?v=lW_JMBWd-c4&index=19&list=RD9gpkOIwCyI8",
                        "https://www.youtube.com/watch?v=qA6sh1FM82w&index=20&list=RD9gpkOIwCyI8",
                        "https://www.youtube.com/watch?v=vApON59CgaM&index=22&list=RD9gpkOIwCyI8",
                        "https://www.youtube.com/watch?v=jxanQSr5T4U&index=23&list=RD9gpkOIwCyI8",
                        "https://www.youtube.com/watch?v=VD9iDZHrQjw&index=24&list=RD9gpkOIwCyI8",
                        "https://www.youtube.com/watch?v=OjP7VTHUais&index=25&list=RD9gpkOIwCyI8",
                        "https://www.youtube.com/watch?v=T4v9PikRxAw&index=26&list=RD9gpkOIwCyI8"
                   };
        }
    }
}

