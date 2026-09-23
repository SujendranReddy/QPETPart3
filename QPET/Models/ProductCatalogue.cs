namespace QPET.Models
{
    public class ProductCatalogueViewModel
    {
        public List<QPET.Data.Entities.Product> Products { get; set; }
            = new List<QPET.Data.Entities.Product>();

        public List<QPET.Data.Entities.Category> Categories { get; set; }
            = new List<QPET.Data.Entities.Category>();

        public List<QPET.Data.Entities.SubCategory> SubCategories { get; set; }
            = new List<QPET.Data.Entities.SubCategory>();
    }
}