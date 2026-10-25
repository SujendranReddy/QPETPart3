
using Microsoft.AspNetCore.Mvc;
using QPET.Models;
using QPET.Services.Interfaces;

namespace QPET.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetProductsAsync(true);

            var capacities = products
                .Select(product => product.CapacityVolume?.Trim())
                .Where(capacity => !string.IsNullOrWhiteSpace(capacity))
                .Select(capacity => capacity!)
                .GroupBy(
                    capacity => string.Concat(
                        capacity.Where(character => !char.IsWhiteSpace(character))
                    ),
                    StringComparer.OrdinalIgnoreCase)
                .Select(group => group.First())
                .OrderBy(capacity => capacity, StringComparer.OrdinalIgnoreCase)
                .ToList();

            var model = new ProductCatalogueViewModel
            {
                Products = products,
                Categories = await _productService.GetCategoriesAsync(),
                SubCategories = await _productService.GetSubCategoriesAsync(),
                Capacities = capacities
            };

            return View(model);
        }
    }
}
