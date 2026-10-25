
namespace QPET.Models
{
    public class ProductCatalogueViewModel
    {
        public List<QPET.Data.Entities.Product> Products { get; set; }
            = new();

        public List<QPET.Data.Entities.Category> Categories { get; set; }
            = new();

        public List<QPET.Data.Entities.SubCategory> SubCategories { get; set; }
            = new();

        public List<string> Capacities { get; set; }
            = new();

        public List<string> NeckFinishes { get; set; }
            = new();

        public string? Search { get; set; }

        public int? SelectedCategoryId { get; set; }

        public int? SelectedSubCategoryId { get; set; }

        public string? SelectedCapacity { get; set; }

        public string? SelectedNeckFinish { get; set; }

        public string? SelectedStatus { get; set; }

        public int TotalProductCount { get; set; }

        public int ActiveProductCount { get; set; }

        public int InactiveProductCount { get; set; }
    }
}
