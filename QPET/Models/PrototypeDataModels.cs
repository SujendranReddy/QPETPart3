namespace QPET.Models
{
    //These models represent the main data used by the prototype, they reflect the entities planned for the final db app. 
    public class PrototypeData
    {
        public List<Branch> Branches { get; set; } = new List<Branch>();
        public List<Category> Categories { get; set; } = new List<Category>();
        public List<SubCategory> SubCategories { get; set; } = new List<SubCategory>();
        public List<Product> Products { get; set; } = new List<Product>();
        public List<Customer> Customers { get; set; } = new List<Customer>();
        public List<Enquiry> Enquiries { get; set; } = new List<Enquiry>();
        public List<Review> Reviews { get; set; } = new List<Review>();
        public PrototypeAdminUser AdminUser { get; set; } = new PrototypeAdminUser();
    }

    public class Branch
    {
        public int BranchId { get; set; }
        public string BranchName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Province { get; set; } = string.Empty;
    }

    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class SubCategory
    {
        public int SubCategoryId { get; set; }
        public int CategoryId { get; set; }
        public string SubCategoryName { get; set; } = string.Empty;
    }

    public class Product
    {
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string NeckFinish { get; set; } = string.Empty;
        public string CapacityVolume { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public List<ProductImage> Images { get; set; } = new List<ProductImage>();
    }

    public class ProductImage
    {
        public int ProductImageId { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public bool IsPrimary { get; set; }
    }

    public class Customer
    {
        public int CustomerId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
    }

    public class Enquiry
    {
        public int EnquiryId { get; set; }
        public int CustomerId { get; set; }
        public int BranchId { get; set; }
        public string Subject { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string CreatedDate { get; set; } = string.Empty;

        //This stores the current stage of the enquiry lifecycle
        public string Status { get; set; } = string.Empty;
        public List<EnquiryAttachment> Attachments { get; set; } = new List<EnquiryAttachment>();
    }

    public class EnquiryAttachment
    {
        public int AttachmentId { get; set; }
        public string OriginalFileName { get; set; } = string.Empty;
        public string StoredFileName { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
    }

    public class Review
    {
        public int ReviewId { get; set; }
        public int BranchId { get; set; }
        public string DisplayName { get; set; } = string.Empty;
        public string? EmailAddress { get; set; }
        public int Rating { get; set; }
        public string ReviewMessage { get; set; } = string.Empty;
        public string CreatedDate { get; set; } = string.Empty;
    }

    public class PrototypeAdminUser
    {
        public string EmailAddress { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}