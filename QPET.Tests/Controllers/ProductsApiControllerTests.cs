using Microsoft.AspNetCore.Mvc;
using Moq;
using QPET.Controllers.Api;
using QPET.Data.Entities;
using QPET.Services.Interfaces;

namespace QPET.Tests.Controllers
{
    public class ProductsApiControllerTests
    {
        [Fact]
        public async Task GetProduct_InvalidId_ReturnsBadRequest()
        {
            var service =
                new Mock<IProductService>();

            var controller =
                new ProductsApiController(
                    service.Object
                );

            var result =
                await controller.GetProduct(0);

            Assert.IsType<
                BadRequestObjectResult>(
                    result.Result
                );
        }

        [Fact]
        public async Task GetProduct_MissingProduct_ReturnsNotFound()
        {
            var service =
                new Mock<IProductService>();

            service
                .Setup(
                    current =>
                        current
                            .GetProductByIdAsync(
                                999
                            )
                )
                .ReturnsAsync(
                    (Product?)null
                );

            var controller =
                new ProductsApiController(
                    service.Object
                );

            var result =
                await controller.GetProduct(
                    999
                );

            Assert.IsType<
                NotFoundObjectResult>(
                    result.Result
                );
        }

        [Fact]
        public async Task DeleteProduct_MissingProduct_ReturnsNotFound()
        {
            var service =
                new Mock<IProductService>();

            service
                .Setup(
                    current =>
                        current
                            .DeleteProductAsync(
                                999
                            )
                )
                .ReturnsAsync(false);

            var controller =
                new ProductsApiController(
                    service.Object
                );

            var result =
                await controller.DeleteProduct(
                    999
                );

            Assert.IsType<
                NotFoundObjectResult>(
                    result
                );
        }
    }
}