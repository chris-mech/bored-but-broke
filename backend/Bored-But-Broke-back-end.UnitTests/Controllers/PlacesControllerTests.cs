using Bored_But_Broke_back_end.Controllers;
using Bored_But_Broke_back_end.Models.Queries;
using Bored_But_Broke_back_end.Models.Responses;
using Bored_But_Broke_back_end.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shouldly;

namespace Bored_But_Broke_back_end.UnitTests.Controllers
{
    internal class PlacesControllerTests
    {
        private Mock<IPlaceService> _mockPlaceService;
        private PlacesController _placesController;

        [SetUp]
        public void Setup()
        {
            _mockPlaceService = new Mock<IPlaceService>();
            _placesController = new PlacesController(_mockPlaceService.Object);
        }

        [Test]
        public async Task GetPlacesAsync_ShouldReturn200WithEmptyList_WhenServiceReturnsEmptyList()
        {
            GetPlacesQuery query = new GetPlacesQuery
            {
                Location = "London"
            };
            var token = CancellationToken.None;

            var response = new PlacesResponse();

            _mockPlaceService
                .Setup(m => m.GetPlacesAsync(query, It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var result = await _placesController.GetPlacesAsync(query, token);

            var okResult = result.ShouldBeOfType<OkObjectResult>();
            okResult.StatusCode.ShouldBe(StatusCodes.Status200OK);
            okResult.Value.ShouldBeEquivalentTo(response);

            _mockPlaceService.Verify(m => m.GetPlacesAsync(query, It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public async Task GetPlacesAsync_ShouldReturn200WithPlaces_WhenServiceReturnsPlaces()
        {
            GetPlacesQuery query = new GetPlacesQuery
            {
                Location = "London"
            };
            var token = CancellationToken.None;

            var response = new PlacesResponse
            {
                Places =
                [
                    new PlaceResponse
                    {
                        PlaceId = "1",
                        PlaceName = "Sample Place"
                    }
                ]
            };

            _mockPlaceService
                .Setup(m => m.GetPlacesAsync(query, It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var result = await _placesController.GetPlacesAsync(query, token);

            var okResult = result.ShouldBeOfType<OkObjectResult>();
            okResult.StatusCode.ShouldBe(StatusCodes.Status200OK);
            okResult.Value.ShouldBeEquivalentTo(response);

            _mockPlaceService.Verify(m => m.GetPlacesAsync(query, It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public async Task GetPlacesAsync_ShouldForwardsCancellationToken()
        {
            using var cts = new CancellationTokenSource();
            var token = cts.Token;

            GetPlacesQuery query = new GetPlacesQuery
            {
                Location = "London"
            };

            _mockPlaceService
                .Setup(m => m.GetPlacesAsync(It.IsAny<GetPlacesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PlacesResponse());

            await _placesController.GetPlacesAsync(query, token);

            _mockPlaceService.Verify(m => m.GetPlacesAsync(It.IsAny<GetPlacesQuery>(), token), Times.Once());
        }
    }
}
