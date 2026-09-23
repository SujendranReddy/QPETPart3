namespace QPET.Models
{
    public class ProductCatalogueViewModel
    {
        public List<Product> Products { get; set; } = new List<Product>();
        public List<Category> Categories { get; set; } = new List<Category>();
        public List<SubCategory> SubCategories { get; set; } = new List<SubCategory>();
    }
}