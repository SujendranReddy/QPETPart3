using System.Text.Json;
using QPET.Models;

namespace QPET.Services
{
    /*
     
    This service acts as the data layer for the prototype. It uses prototype-data.json
    instead of the final db.

     */
    public class PrototypeDataService
    {
        // This prevents simultaneous prototype requests from writing to the json file at the same time.
        private static readonly object FileLock =
            new object();

        private readonly string _filePath;

        private readonly JsonSerializerOptions
            _jsonOptions;


        public PrototypeDataService(
            IWebHostEnvironment environment)
        {
            _filePath =
                Path.Combine(
                    environment.ContentRootPath,
                    "Data",
                    "prototype-data.json"
                );

            _jsonOptions =
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    PropertyNamingPolicy =
                        JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                };
        }


        public List<Branch> GetBranches()
        {
            lock (FileLock)
            {
                return LoadData().Branches;
            }
        }


        public List<Category> GetCategories()
        {
            lock (FileLock)
            {
                return LoadData().Categories;
            }
        }


        public List<SubCategory> GetSubCategories()
        {
            lock (FileLock)
            {
                return LoadData().SubCategories;
            }
        }


        public List<Product> GetProducts()
        {
            lock (FileLock)
            {
                return LoadData().Products;
            }
        }


        public Product? GetProductById(
            int productId)
        {
            lock (FileLock)
            {
                return LoadData()
                    .Products
                    .FirstOrDefault(
                        product =>
                            product.ProductId ==
                            productId
                    );
            }
        }


        public Product AddProduct(
            Product product)
        {
            lock (FileLock)
            {
                var data =
                    LoadData();

                product.ProductId =
                    data.Products.Count == 0
                        ? 1
                        : data.Products.Max(
                            product =>
                                product.ProductId
                        ) + 1;


                var nextImageId =
                    data.Products
                        .SelectMany(
                            product =>
                                product.Images
                        )
                        .Select(
                            image =>
                                image.ProductImageId
                        )
                        .DefaultIfEmpty(0)
                        .Max() + 1;


                if (product.Images.Count == 0)
                {
                    product.Images.Add(
                        new ProductImage
                        {
                            ImageUrl =
                                "/images/products/product-placeholder.jpg",

                            IsPrimary = true
                        }
                    );
                }


                foreach (
                    var image in product.Images)
                {
                    if (
                        image.ProductImageId == 0)
                    {
                        image.ProductImageId =
                            nextImageId++;
                    }
                }


                data.Products.Add(product);

                SaveData(data);

                return product;
            }
        }


        public bool UpdateProduct(
            Product product)
        {
            lock (FileLock)
            {
                var data =
                    LoadData();

                var index =
                    data.Products.FindIndex(
                        item =>
                            item.ProductId ==
                            product.ProductId
                    );


                if (index == -1)
                {
                    return false;
                }


                data.Products[index] =
                    product;

                SaveData(data);

                return true;
            }
        }


        public bool DeleteProduct(
            int productId)
        {
            lock (FileLock)
            {
                var data =
                    LoadData();

                var product =
                    data.Products
                        .FirstOrDefault(
                            product =>
                                product.ProductId ==
                                productId
                        );


                if (product == null)
                {
                    return false;
                }


                data.Products.Remove(
                    product
                );

                SaveData(data);

                return true;
            }
        }


        public List<Customer> GetCustomers()
        {
            lock (FileLock)
            {
                return LoadData().Customers;
            }
        }


        public Customer? FindCustomerByEmail(
            string emailAddress)
        {
            lock (FileLock)
            {
                return LoadData()
                    .Customers
                    .FirstOrDefault(
                        customer =>
                            customer.EmailAddress
                                .Equals(
                                    emailAddress,
                                    StringComparison
                                        .OrdinalIgnoreCase
                                )
                    );
            }
        }


        public Customer AddCustomer(
            Customer customer)
        {
            lock (FileLock)
            {
                var data =
                    LoadData();


                customer.CustomerId =
                    data.Customers.Count == 0
                        ? 1
                        : data.Customers.Max(
                            customer =>
                                customer.CustomerId
                        ) + 1;


                data.Customers.Add(
                    customer
                );

                SaveData(data);

                return customer;
            }
        }


        public List<Enquiry> GetEnquiries()
        {
            lock (FileLock)
            {
                return LoadData().Enquiries;
            }
        }


        public Enquiry? GetEnquiryById(
            int enquiryId)
        {
            lock (FileLock)
            {
                return LoadData()
                    .Enquiries
                    .FirstOrDefault(
                        enquiry =>
                            enquiry.EnquiryId ==
                            enquiryId
                    );
            }
        }


        public Enquiry AddEnquiry(
            Enquiry enquiry)
        {
            lock (FileLock)
            {
                var data =
                    LoadData();


                enquiry.EnquiryId =
                    data.Enquiries.Count == 0
                        ? 1001
                        : data.Enquiries.Max(
                            enquiry =>
                                enquiry.EnquiryId
                        ) + 1;


                enquiry.Status =
                    "New";

                enquiry.CreatedDate =
                    DateTime.Now
                        .ToString("yyyy-MM-dd");


                var nextAttachmentId =
                    data.Enquiries
                        .SelectMany(
                            enquiry =>
                                enquiry.Attachments
                        )
                        .Select(
                            attachment =>
                                attachment.AttachmentId
                        )
                        .DefaultIfEmpty(0)
                        .Max() + 1;


                foreach (
                    var attachment
                    in enquiry.Attachments)
                {
                    attachment.AttachmentId =
                        nextAttachmentId++;
                }


                data.Enquiries.Add(
                    enquiry
                );

                SaveData(data);

                return enquiry;
            }
        }

        // This ensures the enquiry lifecycle transitions on  the severr. 
        public bool UpdateEnquiryStatus(
            int enquiryId,
            string newStatus)
        {
            lock (FileLock)
            {
                var data =
                    LoadData();


                var enquiry =
                    data.Enquiries
                        .FirstOrDefault(
                            enquiry =>
                                enquiry.EnquiryId ==
                                enquiryId
                        );


                if (enquiry == null)
                {
                    return false;
                }


                var allowedTransitions =
                    new Dictionary<string, string?>
                    {
                        {
                            "New",
                            "InProgress"
                        },
                        {
                            "InProgress",
                            "Resolved"
                        },
                        {
                            "Resolved",
                            "Closed"
                        },
                        {
                            "Closed",
                            null
                        }
                    };


                if (
                    !allowedTransitions
                        .TryGetValue(
                            enquiry.Status,
                            out var allowedStatus
                        ) ||
                    allowedStatus != newStatus)
                {
                    return false;
                }


                enquiry.Status =
                    newStatus;

                SaveData(data);

                return true;
            }
        }

        public List<Review> GetReviews()
        {
            lock (FileLock)
            {
                return LoadData().Reviews;
            }
        }


        public Review AddReview(
            Review review)
        {
            lock (FileLock)
            {
                var data =
                    LoadData();


                review.ReviewId =
                    data.Reviews.Count == 0
                        ? 1
                        : data.Reviews.Max(
                            review =>
                                review.ReviewId
                        ) + 1;


                review.CreatedDate =
                    DateTime.Now
                        .ToString("yyyy-MM-dd");


                data.Reviews.Add(
                    review
                );


                SaveData(data);


                return review;
            }
        }


        public bool DeleteReview(
            int reviewId)
        {
            lock (FileLock)
            {
                var data =
                    LoadData();


                var review =
                    data.Reviews
                        .FirstOrDefault(
                            review =>
                                review.ReviewId ==
                                reviewId
                        );


                if (review == null)
                {
                    return false;
                }


                data.Reviews.Remove(
                    review
                );


                SaveData(data);


                return true;
            }
        }

        // This is prototype authentication, the final app will use ASP.NET Core Identity
        public PrototypeAdminUser? AuthenticateAdmin(
    string emailAddress,
    string password)
        {
            lock (FileLock)
            {
                var admin =
                    LoadData().AdminUser;


                if (!admin.IsActive)
                {
                    return null;
                }


                var emailMatches =
                    string.Equals(
                        admin.EmailAddress,
                        emailAddress.Trim(),
                        StringComparison.OrdinalIgnoreCase
                    );


                var passwordMatches =
                    string.Equals(
                        admin.Password,
                        password,
                        StringComparison.Ordinal
                    );


                if (!emailMatches || !passwordMatches)
                {
                    return null;
                }


                return new PrototypeAdminUser
                {
                    EmailAddress =
                        admin.EmailAddress,

                    IsActive =
                        admin.IsActive
                };
            }
        }

        private PrototypeData LoadData()
        {
            if (!File.Exists(_filePath))
            {
                throw new FileNotFoundException(
                    "The prototype data file could not be found.",
                    _filePath
                );
            }


            var json =
                File.ReadAllText(
                    _filePath
                );


            return JsonSerializer
                .Deserialize<PrototypeData>(
                    json,
                    _jsonOptions
                )
                ?? new PrototypeData();
        }


        private void SaveData(
            PrototypeData data)
        {
            var json =
                JsonSerializer.Serialize(
                    data,
                    _jsonOptions
                );


            File.WriteAllText(
                _filePath,
                json
            );
        }
    }
}