namespace DbMergeTool
{
    using System;
    using System.Linq;
    using System.Collections;
    using System.Collections.Generic;
    using System.Drawing.Imaging;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Uspelite.Data;
    using Uspelite.Data.Common.Roles;
    using Uspelite.Data.Models;
    using Uspelite.Data.Models.Enum;
    using Uspelite.Data.Repositories;
    using Uspelite.Services.Common;
    using Uspelite.Services.Data;
    using Uspelite.Services.Data.Contracts;

    public class MergeWorker
    {
        private UspeliteDbContext newDbCtx = new UspeliteDbContext();
        private uspelite_usp1Entities oldDbCtx = new uspelite_usp1Entities();

        public void Start()
        {
            try
            {
                CategoriesService categoriesService = new CategoriesService(new GenericRepository<Category>(this.newDbCtx));
                ImagesService imagesService = new ImagesService(new GenericRepository<Image>(this.newDbCtx), new ImageHelper());
                ArticlesService articlesService = new ArticlesService(new GenericRepository<Article>(this.newDbCtx), categoriesService, imagesService);
                CommentsService commentsService = new CommentsService(new GenericRepository<Comment>(this.newDbCtx));
                RatesService ratesService = new RatesService(new GenericRepository<Rate>(this.newDbCtx));
                UsersService usersService = new UsersService(new GenericRepository<User>(this.newDbCtx));
                VideosService videosService = new VideosService(new GenericRepository<Video>(this.newDbCtx), categoriesService);


                IList<User> usersToAdd = this.GetUsersListFromOldDb(this.oldDbCtx);
                var addedUsers = this.AddUsers(usersToAdd, this.newDbCtx);

                IList<Category> oldCategories = this.GetCategories(this.oldDbCtx);
                IList<Category> addedCategories = this.AddCategories(oldCategories, this.newDbCtx);

                this.MigrateArticles(this.oldDbCtx, this.newDbCtx, imagesService, articlesService, usersService, categoriesService, addedCategories);
            }
            finally
            {
                this.newDbCtx.Dispose();
                this.oldDbCtx.Dispose();
            }
        }

        private void MigrateArticles(uspelite_usp1Entities oldCtx, UspeliteDbContext newCtx, IImagesService imagesService, IArticlesService articlesService, IUsersService usersService, ICategoriesService categoriesService, IList<Category> addedCategories)
        {
            //articleTitle
            //articleContent
            //categoryName --> categoryId

            //SELECT* FROM uspelite_usp1.wp_term_relationships
            //JOIN wp_terms on wp_term_relationships.term_taxonomy_id = wp_terms.term_id
            //JOIN wp_term_taxonomy on wp_term_relationships.term_taxonomy_id = wp_term_taxonomy.term_id
            //WHERE wp_term_relationships.object_id = '307' AND wp_term_taxonomy.taxonomy = 'category'

            var results = (from termRel in oldCtx.wp_term_relationships
                       join wpTerms in oldCtx.wp_terms on termRel.term_taxonomy_id equals wpTerms.term_id
                       join wpTaxonomy in oldCtx.wp_term_taxonomy on termRel.term_taxonomy_id equals wpTaxonomy.term_id
                       where wpTaxonomy.taxonomy == "category"
                       join posts in oldCtx.wp_posts on termRel.object_id equals posts.ID
                       where posts.post_status == "publish" && posts.post_type == "post"
                       join users in oldCtx.wp_users on posts.post_author equals users.ID
                       join postMeta in oldCtx.wp_postmeta on posts.ID equals postMeta.post_id
                       where postMeta.meta_key == "_thumbnail_id"
                            select new ArticlesMergeDto
                            {
                                CategoryName = wpTerms.name,
                                CategorySlug = wpTerms.slug,
                                //CategoryId = categoriesService.GetBySlug(wpTerms.slug),
                                PostId = posts.ID,
                                PostTitle = posts.post_title,
                                PostContent = posts.post_content,
                                PostSlug = posts.post_name,
                                PostCreationDate = posts.post_date,
                               // PostAuthorId = usersService.GetByEmail(users.user_email),
                                PictureInPostsId = postMeta.meta_value,
                                AuthorEmail = users.user_email,
                            })
                             .DistinctBy(x => x.PostId)
                             .ToList();
            //problem with article 299 
            for (int i = 300; i < results.Count; i++)
            {
                var res = results[i];

                res.CategoryId = newDbCtx.Categories.FirstOrDefault(x => x.Title == res.CategoryName).Id;
                res.PostAuthorId = usersService.GetByEmail(res.AuthorEmail).Id;
                res.PostTitleImageUrl = oldCtx.wp_posts.FirstOrDefault(x => x.ID.ToString() == res.PictureInPostsId.ToString()).guid;

                var imageUrl = "";
                var imageTitle = Guid.NewGuid().ToString();
                var userId = res.PostAuthorId;

                var image = imagesService.SaveImageFromWeb(res.PostTitleImageUrl, imageTitle, ImageFormat.Jpeg, userId);
                image.IsMain = true;

                var articleTitle = res.PostTitle;
                var articleSlug = res.PostSlug;
                var articleContent = res.PostContent;
                int categoryId = res.CategoryId;

                var comments = oldCtx.wp_comments
                                     .Where(
                                         x => x.comment_ID == res.PostId && x.user_id != 0 && x.comment_approved == "1")
                                     .Select(x => new { Content = x.comment_content, AuthorEmail = x.comment_author_email })
                                     .ToList();

                if(comments.Count > 0)
                {
                    List<Comment> commentsToAdd = new List<Comment>();
                    foreach (var comm in comments)
                    {
                        var commAuthId = usersService.GetByEmail(comm.AuthorEmail).Id;
                        commentsToAdd.Add(new Comment {Content = comm.Content, AuthorId = commAuthId});
                    }
                    var createdArticleId = articlesService.Add(articleTitle, articleSlug, userId, articleContent, PostStatus.Draft, categoryId, image, commentsToAdd);
                }
                else
                {
                    var createdArticleId = articlesService.Add(articleTitle, articleSlug, userId, articleContent, PostStatus.Draft, categoryId, image);
                }
               
            }

        }



        private IList<Category> AddCategories(IList<Category> oldCategories, UspeliteDbContext context)
        {
            IList<Category> result = new List<Category>();
            foreach (var category in oldCategories)
            {
                if (!context.Categories.Any(x => x.Title == category.Title))
                {
                    context.Categories.Add(category);
                }
                result.Add(category);
            }

            context.SaveChanges();

            return result;
        }

        private IList<Category> GetCategories(uspelite_usp1Entities oldDbctx)
        {
            IList<Category> categories = new List<Category>();

            var taxonomyIds = oldDbctx.wp_term_taxonomy.Where(x => x.taxonomy == "category").Select(x => x.term_id).ToList();

            foreach (var termId in taxonomyIds)
            {
                var cat = oldDbctx.wp_terms.Where(x => x.term_id == termId).Select(x => new { Name = x.name, Slug = x.slug }).FirstOrDefault();
                if (cat != null)
                {
                    categories.Add(new Category() { Title = cat.Name, Slug = cat.Slug });
                }
            }
            return categories;
        }

        private IList<User> GetUsersListFromOldDb(uspelite_usp1Entities oldDbCtx)
        {
            List<User> result = new List<User>();
            var allOldUsers = oldDbCtx.wp_users.ToList();
            var allOldUsersMeta = oldDbCtx.wp_usermeta.ToList();

            foreach (var oldUser in allOldUsers)
            {
                var userNames = oldUser.display_name.Split(new char[] { '-', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                var usernames = oldUser.user_nicename.Split(new char[] { '-', ' ' }, StringSplitOptions.RemoveEmptyEntries);

                var username = string.Empty;
                foreach (var un in usernames)
                {
                    username += un;
                }

                var user = new User();
                user.Email = oldUser.user_email;
                user.FirstName = userNames[0];
                user.LastName = userNames[1];
                user.UserName = username;
                user.CreatedOn = oldUser.user_registered;
                user.ShortInfo = allOldUsersMeta.FirstOrDefault(x => (x.user_id == oldUser.ID) && (x.meta_key == "description")) != null ? allOldUsersMeta.FirstOrDefault(x => (x.user_id == oldUser.ID) && (x.meta_key == "description")).meta_value : null;

                result.Add(user);
            }

            return result;
        }

        private IList<User> AddUsers(IList<User> users, UspeliteDbContext context)
        {
            var userManager = new UserManager<User>(new UserStore<User>(context));
            IList<User> addedUsers = new List<User>();

            foreach (var user in users)
            {
                if (!userManager.Users.Any(x => x.UserName == user.UserName))
                {
                    var result = userManager.Create(user, "defpass123");
                    if (result.Succeeded)
                    {
                        userManager.AddToRole(user.Id, AppRoles.CLIENT_ROLE);
                        userManager.AddToRole(user.Id, AppRoles.EDITOR_ROLE);
                        addedUsers.Add(user);
                    }
                    else
                    {
                        throw new ArgumentException("User not created successfully");
                    }
                }
                else
                {
                    addedUsers.Add(context.Users.FirstOrDefault(x => x.UserName == user.UserName));
                }
            }
            return addedUsers;
        }
    }
}
