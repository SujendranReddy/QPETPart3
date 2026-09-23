using Microsoft.AspNetCore.Mvc;
using QPET.Models;
using QPET.Services.Interfaces;

namespace QPET.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;

        public ProductsController(
            IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var model =
                new ProductCatalogueViewModel
                {
                    Products =
                        await _productService
                            .GetProductsAsync(true),

                    Categories =
                        await _productService
                            .GetCategoriesAsync(),

                    SubCategories =
                        await _productService
                            .GetSubCategoriesAsync()
                };

            return View(model);
        }
    }
}