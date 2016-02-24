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

    public sealed class Configuration : DbMigrationsConfiguration<UspeliteDbContext>
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
            IList<User> seededUsers = new List<User>();

            if (!context.Users.Any())
            {
                var userManager = new UserManager<User>(new UserStore<User>(context));
                this.SeedAppRoles(context);
                seededUsers = this.SeedAppUsers(context, userManager);


                IList<Category> seededCategories = new List<Category>();
                if (!context.Categories.Any())
                {
                    var categoryNames = new string[] { "Новини", "Интервюта", "Статии", "Спорт", "Сцената", "Култура", "По света", "От вас за вас", "Бизнес", "Великите непознати", "Защо избрах българия", "Тайните кътчета", "Вицове", "Изкуство", "Фолклор" };

                    foreach (var name in categoryNames)
                    {
                        var category = new Category { Title = name };
                        context.Categories.Add(category);
                        seededCategories.Add(category);
                    }

                    context.SaveChanges();
                }

                IList<Article> seededArticles = new List<Article>();
                if (!context.Articles.Any())
                {
                    for (int i = 0; i < 5; i++)
                    {
                        var randomAuthor = seededUsers[i % (seededUsers.Count - 1)].Id;

                        var article = new Article()
                        {
                            AuthorId = randomAuthor,
                            Title = "Примерно заглавие на статия #" + i,
                            Content = this.randomGenerator.RandomText(25),
                            CategoryId = seededCategories[i % (seededCategories.Count - 1)].Id,
                        };

                        seededArticles.Add(article);
                        context.Articles.Add(article);

                    }

                    context.SaveChanges();
                }

                IList<Video> seededVideos = new List<Video>();
                if (!context.Videos.Any())
                {
                    var allVideoUrls = this.GetAllVideoUrls();

                    for (int i = 0; i < allVideoUrls.Count; i++)
                    {
                        var videoUrl = allVideoUrls[i];
                        var video = new Video
                        {
                            Author = seededUsers[i % (seededUsers.Count - 1)],
                            Title = "Примерно заглавие на видео #" + i,
                            VideoUrl = videoUrl,
                            CategoryId = seededCategories[this.randomGenerator.RandomIntegerBetween(0, seededCategories.Count - 1)].Id
                        };

                        seededVideos.Add(video);
                        context.Videos.Add(video);
                    }

                    context.SaveChanges();
                }

                if (!context.Rates.Any())
                {
                    for (int i = 0; i < 100; i++)
                    {
                        var randomVideo = seededVideos[this.randomGenerator.RandomIntegerBetween(0, seededVideos.Count - 1)];
                        randomVideo.Ratings.Add(new Rate()
                        {
                            Author = seededUsers[i % (seededUsers.Count - 1)],
                            IsPositive = this.randomGenerator.RandomIntegerBetween(0, 1) != 0,
                            Value = this.randomGenerator.RandomIntegerBetween(1, 5),
                        });
                    }
                    context.SaveChanges();
                    for (int i = 0; i < 50; i++)
                    {
                        var randomArticle = seededArticles[this.randomGenerator.RandomIntegerBetween(0, seededArticles.Count - 1)];
                        randomArticle.Ratings.Add(new Rate()
                        {
                            Author = seededUsers[i % (seededUsers.Count - 1)],
                            IsPositive = this.randomGenerator.RandomIntegerBetween(0, 1) != 0,
                            Value = this.randomGenerator.RandomIntegerBetween(1, 5),
                        });
                    }
                    context.SaveChanges();
                    for (int i = 0; i < 100; i++)
                    {
                        var randomCategory = seededCategories[this.randomGenerator.RandomIntegerBetween(0, seededCategories.Count - 1)];
                        randomCategory.Ratings.Add(new Rate()
                        {
                            Author = seededUsers[i % (seededUsers.Count - 1)],
                            IsPositive = this.randomGenerator.RandomIntegerBetween(0, 1) != 0,
                            Value = this.randomGenerator.RandomIntegerBetween(1, 5),
                        });
                    }
                    context.SaveChanges();
                }

                if (!context.Comments.Any())
                {
                    for (int i = 0; i < 10; i++)
                    {
                        var randomArticle = seededArticles[this.randomGenerator.RandomIntegerBetween(0, seededArticles.Count - 1)];

                        randomArticle.Comments.Add(new Comment
                        {
                            Author = seededUsers[i % (seededUsers.Count - 1)],
                            Content = this.randomGenerator.RandomText(15)
                        });
                    }

                    for (int i = 0; i < 20; i++)
                    {
                        var randomVideo = seededVideos[this.randomGenerator.RandomIntegerBetween(0, seededVideos.Count - 1)];
                        randomVideo.Comments.Add(new Comment
                        {
                            Author = seededUsers[i % (seededUsers.Count - 1)],
                            Content = this.randomGenerator.RandomText(15)
                        });
                        context.SaveChanges();
                    }
                }

                context.SaveChanges();
            }
        }

        private IList<User> SeedAppUsers(UspeliteDbContext context, UserManager<User> userManager)
        {
            var result = new List<User>();
            for (int i = 0; i < 5; i++)
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

