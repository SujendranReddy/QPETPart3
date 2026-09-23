using Microsoft.EntityFrameworkCore;
using QPET.Data.Entities;

namespace QPET.Data
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(QpetDbContext context)
        {
            if (await context.Branches.AnyAsync())
            {
                return;
            }

            var pietermaritzburg = new Branch
            {
                BranchName = "Pietermaritzburg",
                PhoneNumber = "000 000 0000",
                EmailAddress = "pietermaritzburg@qpet.co.za",
                City = "Pietermaritzburg",
                Province = "KwaZulu-Natal"
            };

            var johannesburg = new Branch
            {
                BranchName = "Johannesburg",
                PhoneNumber = "000 000 0000",
                EmailAddress = "johannesburg@qpet.co.za",
                City = "Johannesburg",
                Province = "Gauteng"
            };

            var capeTown = new Branch
            {
                BranchName = "Cape Town",
                PhoneNumber = "000 000 0000",
                EmailAddress = "capetown@qpet.co.za",
                City = "Cape Town",
                Province = "Western Cape"
            };

            var bottles = new Category
            {
                CategoryName = "Bottles",
                Description = "PET bottle packaging solutions."
            };

            var jars = new Category
            {
                CategoryName = "Jars",
                Description = "PET jar packaging solutions."
            };

            var preforms = new Category
            {
                CategoryName = "Preforms",
                Description = "PET preforms for packaging manufacturing."
            };

            var waterBottles = new SubCategory
            {
                SubCategoryName = "Water Bottles",
                Category = bottles
            };

            var juiceBottles = new SubCategory
            {
                SubCategoryName = "Juice Bottles",
                Category = bottles
            };

            var beverageBottles = new SubCategory
            {
                SubCategoryName = "Beverage Bottles",
                Category = bottles
            };

            var foodJars = new SubCategory
            {
                SubCategoryName = "Food Jars",
                Category = jars
            };

            var standardPreforms = new SubCategory
            {
                SubCategoryName = "Standard Preforms",
                Category = preforms
            };

            var product1 = new Product
            {
                ProductName = "500ml PET Water Bottle",
                NeckFinish = "28mm",
                CapacityVolume = "500ml",
                Description = "Clear PET bottle suitable for bottled water.",
                IsActive = true,
                Category = bottles,
                SubCategory = waterBottles,
                Images = new List<ProductImage>
                {
                    new ProductImage
                    {
                        ImageUrl = "/images/products/product-placeholder.jpg",
                        IsPrimary = true
                    }
                }
            };

            var product2 = new Product
            {
                ProductName = "1L PET Juice Bottle",
                NeckFinish = "38mm",
                CapacityVolume = "1L",
                Description = "PET bottle designed for juice and beverage products.",
                IsActive = true,
                Category = bottles,
                SubCategory = juiceBottles,
                Images = new List<ProductImage>
                {
                    new ProductImage
                    {
                        ImageUrl = "/images/products/product-placeholder.jpg",
                        IsPrimary = true
                    }
                }
            };

            var product3 = new Product
            {
                ProductName = "2L PET Beverage Bottle",
                NeckFinish = "28mm",
                CapacityVolume = "2L",
                Description = "Large PET bottle suitable for beverage packaging.",
                IsActive = false,
                Category = bottles,
                SubCategory = beverageBottles,
                Images = new List<ProductImage>
                {
                    new ProductImage
                    {
                        ImageUrl = "/images/products/product-placeholder.jpg",
                        IsPrimary = true
                    }
                }
            };

            var product4 = new Product
            {
                ProductName = "500ml PET Food Jar",
                NeckFinish = "Wide Neck",
                CapacityVolume = "500ml",
                Description = "Wide-neck PET jar suitable for food products.",
                IsActive = true,
                Category = jars,
                SubCategory = foodJars,
                Images = new List<ProductImage>
                {
                    new ProductImage
                    {
                        ImageUrl = "/images/products/product-placeholder.jpg",
                        IsPrimary = true
                    }
                }
            };

            var product5 = new Product
            {
                ProductName = "Standard PET Preform",
                NeckFinish = "28mm",
                CapacityVolume = "Standard",
                Description = "PET preform suitable for bottle manufacturing.",
                IsActive = true,
                Category = preforms,
                SubCategory = standardPreforms,
                Images = new List<ProductImage>
                {
                    new ProductImage
                    {
                        ImageUrl = "/images/products/product-placeholder.jpg",
                        IsPrimary = true
                    }
                }
            };

            var sujendran = new Customer
            {
                FullName = "Sujendran Reddy",
                EmailAddress = "sr@example.com",
                PhoneNumber = "082 111 1111"
            };

            var sarah = new Customer
            {
                FullName = "Sarah Naidoo",
                EmailAddress = "sn@example.com",
                PhoneNumber = "083 222 2222"
            };

            var sajana = new Customer
            {
                FullName = "Sajana Motheram",
                EmailAddress = "sm@example.com",
                PhoneNumber = "084 333 3333"
            };

            var sajanaGmail = new Customer
            {
                FullName = "Sajana Motheram",
                EmailAddress = "sajana@gmail.com",
                PhoneNumber = "+27726361007"
            };

            var enquiries = new[]
            {
                new Enquiry
                {
                    Customer = sujendran,
                    Branch = pietermaritzburg,
                    Subject = "Bulk bottle quotation",
                    Message = "We would like a quotation for 5000 PET water bottles.",
                    CreatedDate = new DateTime(2026, 8, 10),
                    Status = "InProgress",
                    Attachments = new List<EnquiryAttachment>
                    {
                        new EnquiryAttachment
                        {
                            OriginalFileName = "requirements.pdf",
                            StoredFileName = "requirements.pdf",
                            ContentType = "application/pdf"
                        }
                    }
                },
                new Enquiry
                {
                    Customer = sarah,
                    Branch = johannesburg,
                    Subject = "Custom packaging enquiry",
                    Message = "Please provide information about your available bottle sizes.",
                    CreatedDate = new DateTime(2026, 8, 11),
                    Status = "InProgress"
                },
                new Enquiry
                {
                    Customer = sajana,
                    Branch = capeTown,
                    Subject = "Product availability",
                    Message = "I would like more information regarding your PET preforms.",
                    CreatedDate = new DateTime(2026, 8, 12),
                    Status = "Resolved"
                },
                new Enquiry
                {
                    Customer = sujendran,
                    Branch = pietermaritzburg,
                    Subject = "Previous quotation",
                    Message = "Thank you for assisting with our previous quotation.",
                    CreatedDate = new DateTime(2026, 8, 8),
                    Status = "Closed"
                },
                new Enquiry
                {
                    Customer = sajanaGmail,
                    Branch = pietermaritzburg,
                    Subject = "bottle",
                    Message = "bottle",
                    CreatedDate = new DateTime(2026, 8, 15),
                    Status = "Closed"
                }
            };

            var reviews = new[]
            {
                new Review
                {
                    Branch = pietermaritzburg,
                    DisplayName = "Sujendran R.",
                    EmailAddress = "sr@example.com",
                    Rating = 5,
                    ReviewMessage = "Excellent service and quick response.",
                    CreatedDate = new DateTime(2026, 8, 10)
                },
                new Review
                {
                    Branch = johannesburg,
                    DisplayName = "Sarah N.",
                    EmailAddress = null,
                    Rating = 4,
                    ReviewMessage = "Good product quality and helpful staff.",
                    CreatedDate = new DateTime(2026, 8, 11)
                },
                new Review
                {
                    Branch = capeTown,
                    DisplayName = "Sajan M.",
                    EmailAddress = null,
                    Rating = 5,
                    ReviewMessage = "Very helpful when answering our packaging questions.",
                    CreatedDate = new DateTime(2026, 8, 12)
                }
            };

            await context.Branches.AddRangeAsync(
                pietermaritzburg,
                johannesburg,
                capeTown
            );

            await context.Categories.AddRangeAsync(
                bottles,
                jars,
                preforms
            );

            await context.Products.AddRangeAsync(
                product1,
                product2,
                product3,
                product4,
                product5
            );

            await context.Customers.AddRangeAsync(
                sujendran,
                sarah,
                sajana,
                sajanaGmail
            );

            await context.Enquiries.AddRangeAsync(enquiries);
            await context.Reviews.AddRangeAsync(reviews);

            await context.SaveChangesAsync();
        }
    }
}