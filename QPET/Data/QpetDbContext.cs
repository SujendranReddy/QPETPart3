using Microsoft.EntityFrameworkCore;
using QPET.Data.Entities;

namespace QPET.Data
{
    public class QpetDbContext : DbContext
    {
        public QpetDbContext(
            DbContextOptions<QpetDbContext> options)
            : base(options)
        {
        }

        public DbSet<AdminUser> AdminUsers => Set<AdminUser>();
        public DbSet<Branch> Branches => Set<Branch>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<SubCategory> SubCategories => Set<SubCategory>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<ProductImage> ProductImages => Set<ProductImage>();
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Enquiry> Enquiries => Set<Enquiry>();
        public DbSet<EnquiryAttachment> EnquiryAttachments => Set<EnquiryAttachment>();
        public DbSet<Review> Reviews => Set<Review>();

        protected override void OnModelCreating(
            ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AdminUser>()
                .HasIndex(admin => admin.EmailAddress)
                .IsUnique();

            modelBuilder.Entity<Branch>()
                .HasIndex(branch => branch.EmailAddress)
                .IsUnique();

            modelBuilder.Entity<Category>()
                .HasIndex(category => category.CategoryName)
                .IsUnique();

            modelBuilder.Entity<Customer>()
                .HasIndex(customer => customer.EmailAddress)
                .IsUnique();

            modelBuilder.Entity<SubCategory>()
                .HasIndex(subCategory => new
                {
                    subCategory.CategoryId,
                    subCategory.SubCategoryName
                })
                .IsUnique();

            modelBuilder.Entity<SubCategory>()
                .HasOne(subCategory => subCategory.Category)
                .WithMany(category => category.SubCategories)
                .HasForeignKey(subCategory => subCategory.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .HasOne(product => product.Category)
                .WithMany(category => category.Products)
                .HasForeignKey(product => product.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .HasOne(product => product.SubCategory)
                .WithMany(subCategory => subCategory.Products)
                .HasForeignKey(product => product.SubCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProductImage>()
                .HasOne(image => image.Product)
                .WithMany(product => product.Images)
                .HasForeignKey(image => image.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProductImage>()
                .HasIndex(image => image.ProductId)
                .IsUnique()
                .HasFilter("[IsPrimary] = 1");

            modelBuilder.Entity<Enquiry>()
                .HasOne(enquiry => enquiry.Customer)
                .WithMany(customer => customer.Enquiries)
                .HasForeignKey(enquiry => enquiry.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Enquiry>()
                .HasOne(enquiry => enquiry.Branch)
                .WithMany(branch => branch.Enquiries)
                .HasForeignKey(enquiry => enquiry.BranchId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<EnquiryAttachment>()
                .HasOne(attachment => attachment.Enquiry)
                .WithMany(enquiry => enquiry.Attachments)
                .HasForeignKey(attachment => attachment.EnquiryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Review>()
                .HasOne(review => review.Branch)
                .WithMany(branch => branch.Reviews)
                .HasForeignKey(review => review.BranchId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Enquiry>()
                .ToTable(table =>
                    table.HasCheckConstraint(
                        "CK_Enquiries_Status",
                        "[Status] IN ('New', 'InProgress', 'Resolved', 'Closed')"
                    )
                );

            modelBuilder.Entity<Review>()
                .ToTable(table =>
                    table.HasCheckConstraint(
                        "CK_Reviews_Rating",
                        "[Rating] >= 1 AND [Rating] <= 5"
                    )
                );
        }
    }
}
