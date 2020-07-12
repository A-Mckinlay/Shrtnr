using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Shrtnr.Controllers;
using Shrtnr.HashIds;
using ShrtnrTableClient.Model;
using ShrtnrTableClient.Model.Dto;
using ShrtnrTableClient.Repository;
using UrlValidationClient;
using Xunit;

namespace ShrtnrTests.Controllers
{
    public class ShrtnControllerTests
    {
        #region POST /shrtn endpoint
        [Fact]
        public async Task ItShouldReturnBadRequestResultIfNoUrlIsProvidedAsync()
        {
            var loggerMock = new Mock<ILogger<ShrtnController>>();
            var urlValidatorMock = new Mock<IUrlValidator>();
            var hasherMock = new Mock<IHasher>();
            var shrtUrlRepoMock = new Mock<IShrtUrlRepo>();

            var controller = new ShrtnController(
                loggerMock.Object,
                urlValidatorMock.Object,
                shrtUrlRepoMock.Object,
                hasherMock.Object
            );
            var actual = await controller.PostAsync(null);

            Assert.Equal(typeof(BadRequestResult), actual.Result.GetType());
        }

        [Fact]
        public async Task ItShouldCallTheUrlValidatorMockWithTheGivenUrl()
        {
            var url = "www.example.com/example";
            var loggerMock = new Mock<ILogger<ShrtnController>>();
            var urlValidatorMock = new Mock<IUrlValidator>();
            var hasherMock = new Mock<IHasher>();
            var shrtUrlRepoMock = new Mock<IShrtUrlRepo>();

            urlValidatorMock
                .Setup(x => x.ValidateUrl(It.IsAny<string>()))
                .ReturnsAsync(new HttpResponseMessage());

            var controller = new ShrtnController(
                loggerMock.Object,
                urlValidatorMock.Object,
                shrtUrlRepoMock.Object,
                hasherMock.Object
            );
            var actual = await controller.PostAsync(url);

            urlValidatorMock.Verify(m => m.ValidateUrl(url));
        }

        [Fact]
        public async Task ItShouldReturnNotFoundObjectResultWhenTheUrlCanNotBeValidated()
        {
            var url = "www.example.com/example";
            var loggerMock = new Mock<ILogger<ShrtnController>>();
            var urlValidatorMock = new Mock<IUrlValidator>();
            var hasherMock = new Mock<IHasher>();
            var shrtUrlRepoMock = new Mock<IShrtUrlRepo>();

            urlValidatorMock
                .Setup(x => x.ValidateUrl(It.IsAny<string>()))
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = System.Net.HttpStatusCode.NotFound
                });

            var controller = new ShrtnController(
                loggerMock.Object,
                urlValidatorMock.Object,
                shrtUrlRepoMock.Object,
                hasherMock.Object
            );
            var actual = await controller.PostAsync(url);

            Assert.Equal(typeof(NotFoundObjectResult), actual.Result.GetType());
        }

        [Fact]
        public async Task ItShouldCallTheHasherMockToGetACode()
        {
            var url = "www.example.com/example";
            var hash = "Xy12D7";

            var loggerMock = new Mock<ILogger<ShrtnController>>();
            var urlValidatorMock = new Mock<IUrlValidator>();
            var hasherMock = new Mock<IHasher>();
            var shrtUrlRepoMock = new Mock<IShrtUrlRepo>();

            urlValidatorMock
                .Setup(x => x.ValidateUrl(It.IsAny<string>()))
                .ReturnsAsync(new HttpResponseMessage());

            hasherMock
                .Setup(x => x.GetCode())
                .Returns(hash);

            var controller = new ShrtnController(
                loggerMock.Object,
                urlValidatorMock.Object,
                shrtUrlRepoMock.Object,
                hasherMock.Object
            );
            var actual = await controller.PostAsync(url);

            hasherMock.Verify(m => m.GetCode());
        }

        [Fact]
        public async Task ItShouldCallTheMockedAddShrtUrlMethodWithTheUrlHashPair()
        {
            var url = "www.example.com/example";
            var hash = "Xy12D7";
            var shrtUrlEntity = new ShrtUrlEntity(url, hash);
            var urlHashPair = new UrlHashPair()
            {
                Hash = hash,
                Url = url
            };

            var loggerMock = new Mock<ILogger<ShrtnController>>();
            var urlValidatorMock = new Mock<IUrlValidator>();
            var hasherMock = new Mock<IHasher>();
            var shrtUrlRepoMock = new Mock<IShrtUrlRepo>();

            urlValidatorMock
                .Setup(x => x.ValidateUrl(It.IsAny<string>()))
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = System.Net.HttpStatusCode.OK
                });

            hasherMock
                .Setup(x => x.GetCode())
                .Returns(hash);

            shrtUrlRepoMock
                .Setup(x => x.AddShrtUrl(It.IsAny<UrlHashPair>()))
                .Verifiable();

            var controller = new ShrtnController(
                loggerMock.Object,
                urlValidatorMock.Object,
                shrtUrlRepoMock.Object,
                hasherMock.Object
            );
            await controller.PostAsync(url);

            shrtUrlRepoMock.Verify(m => m.AddShrtUrl(
                It.Is<UrlHashPair>(u => u.Url == url
                    && u.Hash == hash
                )
            ));
        }

        [Fact]
        public async Task ItShouldReturnOkObjectResultWithTheUrlHashPair()
        {
            var url = "www.example.com/example";
            var hash = "Xy12D7";
            var shrtUrlEntity = new ShrtUrlEntity(url, hash);
            var urlHashPair = new UrlHashPair()
            {
                Hash = hash,
                Url = url
            };

            var loggerMock = new Mock<ILogger<ShrtnController>>();
            var urlValidatorMock = new Mock<IUrlValidator>();
            var hasherMock = new Mock<IHasher>();
            var shrtUrlRepoMock = new Mock<IShrtUrlRepo>();

            urlValidatorMock
                .Setup(x => x.ValidateUrl(It.IsAny<string>()))
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = System.Net.HttpStatusCode.OK
                });

            hasherMock
                .Setup(x => x.GetCode())
                .Returns(hash);

            shrtUrlRepoMock
                .Setup(x => x.AddShrtUrl(It.IsAny<UrlHashPair>()))
                .ReturnsAsync(shrtUrlEntity);

            var controller = new ShrtnController(
                loggerMock.Object,
                urlValidatorMock.Object,
                shrtUrlRepoMock.Object,
                hasherMock.Object
            );
            var res = await controller.PostAsync(url);

            var expected = new OkObjectResult(new UrlHashPair().FromShrtUrlEntity(shrtUrlEntity));

            Assert.Equal(typeof(OkObjectResult), res.Result.GetType());
            Assert.Equal(urlHashPair.Url, ((UrlHashPair)((OkObjectResult)res.Result).Value).Url);
            Assert.Equal(urlHashPair.Hash, ((UrlHashPair)((OkObjectResult)res.Result).Value).Hash);
        }

        [Fact]
        public async Task ItShouldLogThatAnExceptionWasThrownAndReturn500WhenAnExceptionIsThrown()
        {
            var ex = new Exception();
            var url = "www.example.com/example";

            var loggerMock = new Mock<ILogger<ShrtnController>>();
            var urlValidatorMock = new Mock<IUrlValidator>();
            var hasherMock = new Mock<IHasher>();
            var shrtUrlRepoMock = new Mock<IShrtUrlRepo>();

            urlValidatorMock
                .Setup(x => x.ValidateUrl(It.IsAny<string>()))
                .ThrowsAsync(ex);

            var controller = new ShrtnController(
                loggerMock.Object,
                urlValidatorMock.Object,
                shrtUrlRepoMock.Object,
                hasherMock.Object
            );
            var actual = await controller.PostAsync(url);

            Assert.Equal(typeof(StatusCodeResult), actual.Result.GetType());
            Assert.Equal(500, ((StatusCodeResult)actual.Result).StatusCode);
        }

        #endregion
    }
}
