using Microsoft.AspNetCore.Mvc;
using QPET.Models;
using QPET.Services;

namespace QPET.Controllers
{
    public class ProductsController : Controller
    {
        private readonly PrototypeDataService _dataService;

        public ProductsController(
            PrototypeDataService dataService
        )
        {
            _dataService = dataService;
        }

        public IActionResult Index()
        {
            var model = new ProductCatalogueViewModel
            {
                Products = _dataService
                    .GetProducts()
                    .Where(product => product.IsActive)
                    .ToList(),

                Categories = _dataService.GetCategories(),

                SubCategories = _dataService.GetSubCategories()
            };

            return View(model);
        }
    }
}