using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QPET.Data.Entities;
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

            return Ok(
                products.Select(MapProduct)
            );
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductResponse>>
            GetProduct(
                int id)
        {
            if (id <= 0)
            {
                return BadRequest(
                    new
                    {
                        message =
                            "Product ID must be greater than zero."
                    }
                );
            }

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

            return Ok(
                MapProduct(product)
            );
        }

        [HttpPost]
        [Authorize(
            AuthenticationSchemes = "QPETAdmin",
            Roles = "Admin"
        )]
        public async Task<ActionResult<ProductResponse>>
            CreateProduct(
                ProductCreateRequest request)
        {
            try
            {
                var product =
                    new Product
                    {
                        CategoryId =
                            request.CategoryId,

                        SubCategoryId =
                            request.SubCategoryId,

                        ProductName =
                            request.ProductName.Trim(),

                        NeckFinish =
                            request.NeckFinish.Trim(),

                        CapacityVolume =
                            request.CapacityVolume.Trim(),

                        Description =
                            request.Description.Trim(),

                        IsActive =
                            request.IsActive
                    };

                var createdProduct =
                    await _productService
                        .AddProductAsync(product);

                var result =
                    await _productService
                        .GetProductByIdAsync(
                            createdProduct.ProductId
                        );

                return CreatedAtAction(
                    nameof(GetProduct),
                    new
                    {
                        id =
                            createdProduct.ProductId
                    },
                    MapProduct(result!)
                );
            }
            catch (ArgumentException exception)
            {
                return BadRequest(
                    new
                    {
                        message =
                            exception.Message
                    }
                );
            }
        }

        [HttpPut("{id:int}")]
        [Authorize(
            AuthenticationSchemes = "QPETAdmin",
            Roles = "Admin"
        )]
        public async Task<IActionResult> UpdateProduct(
            int id,
            ProductUpdateRequest request)
        {
            if (id <= 0)
            {
                return BadRequest(
                    new
                    {
                        message =
                            "Product ID must be greater than zero."
                    }
                );
            }

            var existingProduct =
                await _productService
                    .GetProductByIdAsync(id);

            if (existingProduct == null)
            {
                return NotFound(
                    new
                    {
                        message =
                            "Product not found."
                    }
                );
            }

            try
            {
                var product =
                    new Product
                    {
                        ProductId =
                            id,

                        CategoryId =
                            request.CategoryId,

                        SubCategoryId =
                            request.SubCategoryId,

                        ProductName =
                            request.ProductName.Trim(),

                        NeckFinish =
                            request.NeckFinish.Trim(),

                        CapacityVolume =
                            request.CapacityVolume.Trim(),

                        Description =
                            request.Description.Trim(),

                        IsActive =
                            request.IsActive
                    };

                var updated =
                    await _productService
                        .UpdateProductAsync(product);

                if (!updated)
                {
                    return NotFound(
                        new
                        {
                            message =
                                "Product not found."
                        }
                    );
                }

                return NoContent();
            }
            catch (ArgumentException exception)
            {
                return BadRequest(
                    new
                    {
                        message =
                            exception.Message
                    }
                );
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize(
            AuthenticationSchemes = "QPETAdmin",
            Roles = "Admin"
        )]
        public async Task<IActionResult> DeleteProduct(
            int id)
        {
            if (id <= 0)
            {
                return BadRequest(
                    new
                    {
                        message =
                            "Product ID must be greater than zero."
                    }
                );
            }

            var deleted =
                await _productService
                    .DeleteProductAsync(id);

            if (!deleted)
            {
                return NotFound(
                    new
                    {
                        message =
                            "Product not found."
                    }
                );
            }

            return NoContent();
        }

        private static ProductResponse MapProduct(
            Product product)
        {
            return new ProductResponse
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
                    product.Category?
                        .CategoryName
                    ?? string.Empty,

                SubCategoryId =
                    product.SubCategoryId,

                SubCategoryName =
                    product.SubCategory?
                        .SubCategoryName
                    ?? string.Empty,

                PrimaryImageUrl =
                    product.Images
                        .FirstOrDefault(
                            image =>
                                image.IsPrimary
                        )
                        ?.ImageUrl
            };
        }
    }
}