using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QPET.Models;
using QPET.Services;
using System.Security.Claims;

namespace QPET.Controllers
{
    [Authorize(AuthenticationSchemes = "QPETAdmin")]
    public class AdminController : Controller
    {
        private readonly PrototypeDataService _dataService;

        public AdminController(
            PrototypeDataService dataService)
        {
            _dataService = dataService;
        }


        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login(
    string? returnUrl = null)
        {
            if (
                User.Identity?.IsAuthenticated
                == true)
            {
                return RedirectToAction(
                    nameof(Dashboard)
                );
            }


            return View(
                new AdminLoginViewModel
                {
                    ReturnUrl =
                        returnUrl
                }
            );
        }


        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(
            AdminLoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }


            var admin =
                _dataService.AuthenticateAdmin(
                    model.EmailAddress,
                    model.Password
                );


            if (admin == null)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Invalid email address or password."
                );


                return View(model);
            }


            var claims =
                new List<Claim>
                {
            new Claim(
                ClaimTypes.Name,
                admin.EmailAddress
            ),

            new Claim(
                ClaimTypes.Role,
                "Admin"
            )
                };


            var identity =
                new ClaimsIdentity(
                    claims,
                    "QPETAdmin"
                );


            var principal =
                new ClaimsPrincipal(
                    identity
                );


            var authenticationProperties =
                new AuthenticationProperties
                {
                    IsPersistent =
                        model.RememberMe,

                    ExpiresUtc =
                        DateTimeOffset.UtcNow.Add(
                            model.RememberMe
                                ? TimeSpan.FromDays(7)
                                : TimeSpan.FromHours(8)
                        )
                };


            await HttpContext.SignInAsync(
                "QPETAdmin",
                principal,
                authenticationProperties
            );

            //This reutrns the admin to the requested admin page after a successful login
            if (
                !string.IsNullOrWhiteSpace(
                    model.ReturnUrl
                ) &&
                Url.IsLocalUrl(
                    model.ReturnUrl
                ))
            {
                return LocalRedirect(
                    model.ReturnUrl
                );
            }


            return RedirectToAction(
                nameof(Dashboard)
            );
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                "QPETAdmin"
            );


            return RedirectToAction(
                nameof(Login)
            );
        }


        public IActionResult Dashboard()
        {
            var products =
                _dataService.GetProducts();

            var enquiries =
                _dataService.GetEnquiries();

            var reviews =
                _dataService.GetReviews();

            var customers =
                _dataService.GetCustomers();

            var branches =
                _dataService.GetBranches();


            var recentEnquiries =
                enquiries
                    .OrderByDescending(
                        enquiry =>
                            enquiry.EnquiryId
                    )
                    .Take(5)
                    .Select(
                        enquiry =>
                        {
                            var customer =
                                customers.FirstOrDefault(
                                    customer =>
                                        customer.CustomerId ==
                                        enquiry.CustomerId
                                );


                            var branch =
                                branches.FirstOrDefault(
                                    branch =>
                                        branch.BranchId ==
                                        enquiry.BranchId
                                );


                            return new
                                AdminDashboardEnquiryViewModel
                            {
                                EnquiryId =
                                    enquiry.EnquiryId,

                                CustomerName =
                                    customer?.FullName
                                    ?? "Unknown customer",

                                CustomerEmail =
                                    customer?.EmailAddress
                                    ?? "Email not available",

                                BranchName =
                                    branch?.BranchName
                                    ?? "Unknown branch",

                                Subject =
                                    enquiry.Subject,

                                CreatedDate =
                                    enquiry.CreatedDate,

                                Status =
                                    enquiry.Status
                            };
                        }
                    )
                    .ToList();


            var model =
                new AdminDashboardViewModel
                {
                    ActiveProductCount =
                        products.Count(
                            product =>
                                product.IsActive
                        ),

                    NewEnquiryCount =
                        enquiries.Count(
                            enquiry =>
                                enquiry.Status == "New"
                        ),

                    OpenEnquiryCount =
                        enquiries.Count(
                            enquiry =>
                                enquiry.Status == "New" ||
                                enquiry.Status == "InProgress"
                        ),

                    ReviewCount =
                        reviews.Count,

                    RecentEnquiries =
                        recentEnquiries
                };


            return View(model);
        }


        public IActionResult Enquiries(
            string? status = null)
        {
            var allowedStatuses =
                new[]
                {
                    "New",
                    "InProgress",
                    "Resolved",
                    "Closed"
                };


            if (
                !string.IsNullOrWhiteSpace(status) &&
                !allowedStatuses.Contains(status))
            {
                status = null;
            }


            var enquiries =
                _dataService
                    .GetEnquiries()
                    .OrderByDescending(
                        enquiry =>
                            enquiry.EnquiryId
                    )
                    .AsEnumerable();


            if (!string.IsNullOrWhiteSpace(status))
            {
                enquiries =
                    enquiries.Where(
                        enquiry =>
                            enquiry.Status == status
                    );
            }


            ViewBag.Customers =
                _dataService.GetCustomers();

            ViewBag.Branches =
                _dataService.GetBranches();


            var model =
                new AdminEnquiryListViewModel
                {
                    Enquiries =
                        enquiries.ToList(),

                    SelectedStatus =
                        status
                };


            return View(model);
        }


        [HttpGet]
        public IActionResult EnquiryDetails(
            int id)
        {
            var enquiry =
                _dataService
                    .GetEnquiryById(id);


            if (enquiry == null)
            {
                ViewBag.EnquiryNotFound =
                    true;

                return View(
                    new Enquiry()
                );
            }


            ViewBag.Customer =
                _dataService
                    .GetCustomers()
                    .FirstOrDefault(
                        customer =>
                            customer.CustomerId ==
                            enquiry.CustomerId
                    );


            ViewBag.Branch =
                _dataService
                    .GetBranches()
                    .FirstOrDefault(
                        branch =>
                            branch.BranchId ==
                            enquiry.BranchId
                    );


            return View(enquiry);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateEnquiryStatus(
            int id,
            string status)
        {
            //This service validates that the requested enquiry status change is allowed
            var updated =
                _dataService
                    .UpdateEnquiryStatus(
                        id,
                        status
                    );


            TempData["EnquiryMessage"] =
                updated
                    ? "Enquiry status updated successfully."
                    : "The enquiry status could not be updated.";


            return RedirectToAction(
                nameof(EnquiryDetails),
                new
                {
                    id
                }
            );
        }


        public IActionResult Products()
        {
            var model =
                new ProductCatalogueViewModel
                {
                    Products =
                        _dataService.GetProducts(),

                    Categories =
                        _dataService.GetCategories(),

                    SubCategories =
                        _dataService.GetSubCategories()
                };


            return View(model);
        }


        [HttpGet]
        public IActionResult CreateProduct()
        {
            PopulateProductLookups();


            return View(
                new AdminProductViewModel
                {
                    IsActive = true
                }
            );
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateProduct(
            AdminProductViewModel model)
        {
            ValidateProductSelection(model);
            ValidateProductImage(model);


            if (!ModelState.IsValid)
            {
                PopulateProductLookups();

                return View(model);
            }


            var product =
                new Product
                {
                    CategoryId =
                        model.CategoryId,

                    SubCategoryId =
                        model.SubCategoryId!.Value,

                    ProductName =
                        model.ProductName.Trim(),

                    NeckFinish =
                        model.NeckFinish.Trim(),

                    CapacityVolume =
                        model.CapacityVolume.Trim(),

                    Description =
                        model.Description.Trim(),

                    IsActive =
                        model.IsActive
                };


            var createdProduct =
                _dataService.AddProduct(
                    product
                );


            TempData["ProductMessage"] =
                $"Product created successfully. Product ID: {createdProduct.ProductId}";


            return RedirectToAction(
                nameof(Products)
            );
        }


        [HttpGet]
        public IActionResult EditProduct(
            int id)
        {
            var product =
                _dataService
                    .GetProductById(id);


            if (product == null)
            {
                ViewBag.ProductNotFound =
                    true;

                return View(
                    new AdminProductViewModel()
                );
            }


            PopulateProductLookups();


            var primaryImage =
                product.Images
                    .FirstOrDefault(
                        image =>
                            image.IsPrimary
                    );


            var model =
                new AdminProductViewModel
                {
                    ProductId =
                        product.ProductId,

                    CategoryId =
                        product.CategoryId,

                    SubCategoryId =
                        product.SubCategoryId,

                    ProductName =
                        product.ProductName,

                    NeckFinish =
                        product.NeckFinish,

                    CapacityVolume =
                        product.CapacityVolume,

                    Description =
                        product.Description,

                    IsActive =
                        product.IsActive,

                    ExistingImageUrl =
                        primaryImage?.ImageUrl
                };


            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditProduct(
            int id,
            AdminProductViewModel model)
        {
            var existingProduct =
                _dataService
                    .GetProductById(id);


            if (existingProduct == null)
            {
                return NotFound();
            }


            model.ProductId =
                id;


            ValidateProductSelection(model);
            ValidateProductImage(model);


            if (!ModelState.IsValid)
            {
                PopulateProductLookups();


                model.ExistingImageUrl =
                    existingProduct
                        .Images
                        .FirstOrDefault(
                            image =>
                                image.IsPrimary
                        )
                        ?.ImageUrl;


                return View(model);
            }


            var updatedProduct =
                new Product
                {
                    ProductId =
                        id,

                    CategoryId =
                        model.CategoryId,

                    SubCategoryId =
                        model.SubCategoryId!.Value,

                    ProductName =
                        model.ProductName.Trim(),

                    NeckFinish =
                        model.NeckFinish.Trim(),

                    CapacityVolume =
                        model.CapacityVolume.Trim(),

                    Description =
                        model.Description.Trim(),

                    IsActive =
                        model.IsActive,

                    Images =
                        existingProduct.Images
                };


            _dataService.UpdateProduct(
                updatedProduct
            );


            TempData["ProductMessage"] =
                "Product updated successfully.";


            return RedirectToAction(
                nameof(Products)
            );
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteProduct(
            int id)
        {
            var deleted =
                _dataService
                    .DeleteProduct(id);


            TempData["ProductMessage"] =
                deleted
                    ? "Product deleted successfully."
                    : "The product could not be found.";


            return RedirectToAction(
                nameof(Products)
            );
        }


        public IActionResult Reviews()
        {
            var reviews =
                _dataService
                    .GetReviews()
                    .OrderByDescending(
                        review =>
                            review.ReviewId
                    )
                    .ToList();


            ViewBag.Branches =
                _dataService
                    .GetBranches();


            return View(
                reviews
            );
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteReview(
            int id)
        {
            var deleted =
                _dataService
                    .DeleteReview(id);


            TempData["ReviewMessage"] =
                deleted
                    ? "Review deleted successfully."
                    : "The review could not be found.";


            return RedirectToAction(
                nameof(Reviews)
            );
        }


        private void PopulateProductLookups()
        {
            ViewBag.Categories =
                _dataService.GetCategories();

            ViewBag.SubCategories =
                _dataService.GetSubCategories();
        }


        private void ValidateProductSelection(
            AdminProductViewModel model)
        {
            var categories =
                _dataService.GetCategories();

            var subCategories =
                _dataService.GetSubCategories();


            if (
                !categories.Any(
                    category =>
                        category.CategoryId ==
                        model.CategoryId
                ))
            {
                ModelState.AddModelError(
                    nameof(model.CategoryId),
                    "Select a valid category."
                );
            }


            if (
                !model.SubCategoryId.HasValue ||
                !subCategories.Any(
                    subCategory =>
                        subCategory.SubCategoryId ==
                        model.SubCategoryId.Value &&
                        subCategory.CategoryId ==
                        model.CategoryId
                ))
            {
                ModelState.AddModelError(
                    nameof(model.SubCategoryId),
                    "Select a valid subcategory."
                );
            }
        }


        private void ValidateProductImage(
            AdminProductViewModel model)
        {
            if (model.Image == null)
            {
                return;
            }


            var extension =
                Path.GetExtension(
                    model.Image.FileName
                )
                .ToLowerInvariant();


            var allowedExtensions =
                new[]
                {
                    ".jpg",
                    ".jpeg",
                    ".png",
                    ".webp"
                };


            if (
                !allowedExtensions.Contains(
                    extension
                ))
            {
                ModelState.AddModelError(
                    nameof(model.Image),
                    "Only JPG, JPEG, PNG and WEBP images are allowed."
                );
            }
        }
    }
}