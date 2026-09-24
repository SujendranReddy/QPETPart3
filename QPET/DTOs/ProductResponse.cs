namespace QPET.DTOs
{
    public class ProductResponse
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }
            = string.Empty;

        public string NeckFinish { get; set; }
            = string.Empty;

        public string CapacityVolume { get; set; }
            = string.Empty;

        public string Description { get; set; }
            = string.Empty;

        public bool IsActive { get; set; }

        public int CategoryId { get; set; }

        public string CategoryName { get; set; }
            = string.Empty;

        public int SubCategoryId { get; set; }

        public string SubCategoryName { get; set; }
            = string.Empty;

        public string? PrimaryImageUrl { get; set; }
    }
}