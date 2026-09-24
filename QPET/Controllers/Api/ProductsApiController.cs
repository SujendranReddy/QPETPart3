using Microsoft.AspNetCore.Mvc;
using QPET.DTOs;
using QPET.Services.Interfaces;

namespace QPET.Controllers.Api
{
    [ApiController]
    [Route("api/products")]
    public class ProductsApiController
        : ControllerBase
    {
        private readonly IProductService
            _productService;

        public ProductsApiController(
            IProductService productService)
        {
            _productService =
                productService;
        }

        [HttpGet]
        public async Task<
            ActionResult<IEnumerable<ProductResponse>>>
            GetProducts()
        {
            var products =
                await _productService
                    .GetProductsAsync(true);

            var response =
                products
                    .Select(
                        product =>
                            new ProductResponse
                            {
                                ProductId =
                                    product.ProductId,

                                ProductName =
                                    product.ProductName,

                                NeckFinish =
                                    product.NeckFinish,

                                CapacityVolume =
                                    product.CapacityVolume,

                                Description =
                                    product.Description,

                                IsActive =
                                    product.IsActive,

                                CategoryId =
                                    product.CategoryId,

                                CategoryName =
                                    product.Category
                                        .CategoryName,

                                SubCategoryId =
                                    product.SubCategoryId,

                                SubCategoryName =
                                    product.SubCategory
                                        .SubCategoryName,

                                PrimaryImageUrl =
                                    product.Images
                                        .FirstOrDefault(
                                            image =>
                                                image.IsPrimary
                                        )
                                        ?.ImageUrl
                            }
                    )
                    .ToList();

            return Ok(response);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductResponse>>
            GetProduct(
                int id)
        {
            var product =
                await _productService
                    .GetProductByIdAsync(id);

            if (
                product == null ||
                !product.IsActive)
            {
                return NotFound(
                    new
                    {
                        message =
                            "Product not found."
                    }
                );
            }

            var response =
                new ProductResponse
                {
                    ProductId =
                        product.ProductId,

                    ProductName =
                        product.ProductName,

                    NeckFinish =
                        product.NeckFinish,

                    CapacityVolume =
                        product.CapacityVolume,

                    Description =
                        product.Description,

                    IsActive =
                        product.IsActive,

                    CategoryId =
                        product.CategoryId,

                    CategoryName =
                        product.Category
                            .CategoryName,

                    SubCategoryId =
                        product.SubCategoryId,

                    SubCategoryName =
                        product.SubCategory
                            .SubCategoryName,

                    PrimaryImageUrl =
                        product.Images
                            .FirstOrDefault(
                                image =>
                                    image.IsPrimary
                            )
                            ?.ImageUrl
                };

            return Ok(response);
        }
    }
}