namespace Uspelite.Web.Controllers.Tests
{
    using System;
    using System.Linq.Expressions;
    using Data.Models;
    using Infrastructure.Mapping;
    using Moq;
    using TestStack.FluentMVCTesting;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Models.Articles;
    using Services.Data.Contracts;
    using Services.Web.Contracts;
    using System.Net;
    using System.Web.Mvc;

    [TestClass]
    public class ArticlesControllerTests
    {
        private ArticlesController controller;

        private Mock<IHttpCacheService> mockedCache;
        private Mock<IArticlesService> mockedArticleService;
        private Mock<ICategoriesService> mockedCategoriesService;
        private Mock<IImagesService> mockedImagesService;

        [TestInitialize]
        public void Innitialize()
        {
            var autoMapperConfig = new AutoMapperConfig();
            autoMapperConfig.Execute(typeof(ArticlesController).Assembly);

            var cacheServiceMock = new Mock<IHttpCacheService>();
            cacheServiceMock
                .Setup(x => x.Get(It.IsAny<string>(), It.IsAny<Expression<Func<Article, bool>>>, It.IsAny<int>()));


            const string ArticleContent = "Test content";
            var articlesServiceMock = new Mock<IArticlesService>();
            articlesServiceMock.Setup(x => x.GetById(It.IsAny<int>()))
                .Returns(new Article { Content = ArticleContent });

            var categoriesServiceMock = new Mock<ICategoriesService>();
            var imagesServiceMock = new Mock<IImagesService>();
            var iSlugServiceMock = new Mock<ISlugService>();
            var iShliokavitsaServiceMock = new Mock<IShliokavitsaConvertService>();

            this.mockedCache = cacheServiceMock;
            this.mockedArticleService = articlesServiceMock;
            this.mockedCategoriesService = categoriesServiceMock;
            this.mockedImagesService = imagesServiceMock;


            this.controller = new ArticlesController(this.mockedArticleService.Object, this.mockedCategoriesService.Object, this.mockedImagesService.Object, iSlugServiceMock.Object, iShliokavitsaServiceMock.Object);
            this.controller.Cache = this.mockedCache.Object;
        }

        [TestMethod]
        public void ControllerAddShouldRenderProperView()
        {
            this.controller.WithCallTo(x => x.Add(string.Empty)).ShouldRenderView("Add");
        }

        [TestMethod]
        public void ControllerShowShouldReturnHttpNotFound()
        {
            this.controller.WithCallTo(x => x.Show("1231a3sadad@343112!asdasdasda-fake-notfound-slug"))
                .ShouldGiveHttpStatus(HttpStatusCode.NotFound);
        }

    }
}
