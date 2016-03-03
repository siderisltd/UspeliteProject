namespace Uspelite.Web.Controllers.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Net;
    using Data.Models;
    using Infrastructure.Mapping;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Models.Authors;
    using Moq;
    using Services.Data.Contracts;
    using Services.Web.Contracts;
    using TestStack.FluentMVCTesting;

    [TestClass]
    public class AuthorsControllerTests
    {
        public AuthorsController controller;

        [TestInitialize]
        public void Innitialize()
        {
            var autoMapperConfig = new AutoMapperConfig();
            autoMapperConfig.Execute(typeof(AuthorsController).Assembly);

            var cacheServiceMock = new Mock<IHttpCacheService>();
            cacheServiceMock
                .Setup(x => x.Get(It.IsAny<string>(), It.IsAny<Expression<Func<Article, bool>>>, It.IsAny<int>()));

            var usersServiceMock = new Mock<IUsersService>();
            usersServiceMock.Setup(x =>
                x.Find(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                            .Returns(new User() { Id= "asdasdasd", FirstName = "Gosho", LastName = "Peshev", ShortInfo = "asdad", ProfileImages = new List<Image> { new Image { IsMainProfilePicture = true} } });


            this.controller = new AuthorsController(usersServiceMock.Object);
            this.controller.Cache = cacheServiceMock.Object;
        }

        [TestMethod]
        public void ControllerShouldReturnNotFoundToUserWithOneName()
        {
            this.controller.WithCallTo(x => x.Info("asdsad", "nasdada"))
                .ShouldGiveHttpStatus(HttpStatusCode.NotFound);
        }

        [TestMethod]
        public void ControllerShouldFindUserAndRenderProperView()
        {
            this.controller.WithCallTo(x => x.Info("asdsad", "nasdada aasdadas"))
                .ShouldRenderDefaultView();
        }

        [TestMethod]
        public void asd()
        {
            this.controller.WithCallTo(x => x.Info("asdsad", ""))
                .ShouldGiveHttpStatus(HttpStatusCode.NotFound);
        }

    }
}
