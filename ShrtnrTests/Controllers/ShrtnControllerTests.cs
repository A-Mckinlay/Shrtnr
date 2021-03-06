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

            Assert.Equal(typeof(BadRequestResult), actual.GetType());
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

            Assert.Equal(typeof(NotFoundObjectResult), actual.GetType());
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
                .Setup(x => x.AddShrtUrl(It.IsAny<ShrtUrlDto>()))
                .Verifiable();

            var controller = new ShrtnController(
                loggerMock.Object,
                urlValidatorMock.Object,
                shrtUrlRepoMock.Object,
                hasherMock.Object
            );
            await controller.PostAsync(url);

            shrtUrlRepoMock.Verify(m => m.AddShrtUrl(
                It.Is<ShrtUrlDto>(u => u.Url == url
                    && u.Code == hash
                )
            ));
        }

        [Fact]
        public async Task ItShouldReturnOkObjectResultWithTheShrtUrlDto()
        {
            var url = "www.example.com/example";
            var hash = "Xy12D7";
            var shrtUrlEntity = new ShrtUrlEntity(hash) { Url = url };
            var shrtUrlDto = new ShrtUrlDto().FromShrtUrlEntity(shrtUrlEntity);

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
                .Setup(x => x.AddShrtUrl(It.IsAny<ShrtUrlDto>()))
                .ReturnsAsync(shrtUrlDto);

            var controller = new ShrtnController(
                loggerMock.Object,
                urlValidatorMock.Object,
                shrtUrlRepoMock.Object,
                hasherMock.Object
            );
            var res = await controller.PostAsync(url);

            var expected = new OkObjectResult(new ShrtUrlDto().FromShrtUrlEntity(shrtUrlEntity));

            Assert.Equal(typeof(OkObjectResult), res.GetType());
            Assert.Equal(shrtUrlDto.Url, ((ShrtUrlDto)((OkObjectResult)res).Value).Url);
            Assert.Equal(shrtUrlDto.Code, ((ShrtUrlDto)((OkObjectResult)res).Value).Code);
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

            Assert.Equal(typeof(StatusCodeResult), actual.GetType());
            Assert.Equal(500, ((StatusCodeResult)actual).StatusCode);
        }

        #endregion
    }
}
