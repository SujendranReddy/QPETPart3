using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QPET.Models;
using QPET.Services;
using System.Security.Claims;
using QPET.Services.Interfaces;

namespace QPET.Controllers
{
    [Authorize(AuthenticationSchemes = "QPETAdmin")]
    public class AdminController : Controller
    {
        private readonly PrototypeDataService _dataService;
        private readonly IProductService _productService;

        private readonly IReviewService _reviewService;
        private readonly IBranchService _branchService;
        private readonly IEnquiryService _enquiryService;

        public AdminController(
     PrototypeDataService dataService,
     IProductService productService, IBranchService branchService,
IEnquiryService enquiryService, IReviewService reviewService)
        {
            _branchService = branchService;
            _enquiryService = enquiryService;
            _reviewService = reviewService;
            _dataService = dataService;
            _productService = productService;
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


        public async Task<IActionResult> Dashboard()
        {
            var products =
                await _productService
                    .GetProductsAsync();

            var enquiries =
                await _enquiryService
                    .GetEnquiriesAsync();

            var reviews =
                await _reviewService
                    .GetReviewsAsync();

            var recentEnquiries =
                enquiries
                    .OrderByDescending(
                        enquiry =>
                            enquiry.EnquiryId
                    )
                    .Take(5)
                    .Select(
                        enquiry =>
                            new AdminDashboardEnquiryViewModel
                            {
                                EnquiryId =
                                    enquiry.EnquiryId,

                                CustomerName =
                                    enquiry.Customer?.FullName
                                    ?? "Unknown customer",

                                CustomerEmail =
                                    enquiry.Customer?.EmailAddress
                                    ?? "Email not available",

                                BranchName =
                                    enquiry.Branch?.BranchName
                                    ?? "Unknown branch",

                                Subject =
                                    enquiry.Subject,

                                CreatedDate =
                                    enquiry.CreatedDate
                                        .ToString("dd MMM yyyy"),

                                Status =
                                    enquiry.Status
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


        public async Task<IActionResult> Enquiries(
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
                await _enquiryService
                    .GetEnquiriesAsync();

            if (!string.IsNullOrWhiteSpace(status))
            {
                enquiries =
                    enquiries
                        .Where(
                            enquiry =>
                                enquiry.Status == status
                        )
                        .ToList();
            }

            ViewBag.Customers =
                await _enquiryService
                    .GetCustomersAsync();

            ViewBag.Branches =
                await _branchService
                    .GetBranchesAsync();

            var model =
                new AdminEnquiryListViewModel
                {
                    Enquiries = enquiries,
                    SelectedStatus = status
                };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EnquiryDetails(
            int id)
        {
            var enquiry =
                await _enquiryService
                    .GetEnquiryByIdAsync(id);

            if (enquiry == null)
            {
                ViewBag.EnquiryNotFound = true;

                return View(
                    new QPET.Data.Entities.Enquiry()
                );
            }

            ViewBag.Customer =
                enquiry.Customer;

            ViewBag.Branch =
                enquiry.Branch;

            return View(enquiry);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateEnquiryStatus(
            int id,
            string status)
        {
            var updated =
                await _enquiryService
                    .UpdateEnquiryStatusAsync(
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


        public async Task<IActionResult> Products()
        {
            var model =
                new ProductCatalogueViewModel
                {
                    Products =
                        await _productService
                            .GetProductsAsync(),

                    Categories =
                        await _productService
                            .GetCategoriesAsync(),

                    SubCategories =
                        await _productService
                            .GetSubCategoriesAsync()
                };

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> CreateProduct()
        {
            await PopulateProductLookupsAsync();

            return View(
                new AdminProductViewModel
                {
                    IsActive = true
                }
            );
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct(
            AdminProductViewModel model)
        {
            await ValidateProductSelectionAsync(model);
            ValidateProductImage(model);

            if (!ModelState.IsValid)
            {
                await PopulateProductLookupsAsync();

                return View(model);
            }

            var product =
                new QPET.Data.Entities.Product
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
                await _productService
                    .AddProductAsync(product);

            TempData["ProductMessage"] =
                $"Product created successfully. Product ID: {createdProduct.ProductId}";

            return RedirectToAction(
                nameof(Products)
            );
        }

        [HttpGet]
        public async Task<IActionResult> EditProduct(
            int id)
        {
            var product =
                await _productService
                    .GetProductByIdAsync(id);

            if (product == null)
            {
                ViewBag.ProductNotFound =
                    true;

                return View(
                    new AdminProductViewModel()
                );
            }

            await PopulateProductLookupsAsync();

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
        public async Task<IActionResult> EditProduct(
            int id,
            AdminProductViewModel model)
        {
            var existingProduct =
                await _productService
                    .GetProductByIdAsync(id);

            if (existingProduct == null)
            {
                return NotFound();
            }

            model.ProductId =
                id;

            await ValidateProductSelectionAsync(model);
            ValidateProductImage(model);

            if (!ModelState.IsValid)
            {
                await PopulateProductLookupsAsync();

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
                new QPET.Data.Entities.Product
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
                        model.IsActive
                };

            var updated =
                await _productService
                    .UpdateProductAsync(
                        updatedProduct
                    );

            TempData["ProductMessage"] =
                updated
                    ? "Product updated successfully."
                    : "The product could not be updated.";

            return RedirectToAction(
                nameof(Products)
            );
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProduct(
            int id)
        {
            var deleted =
                await _productService
                    .DeleteProductAsync(id);

            TempData["ProductMessage"] =
                deleted
                    ? "Product deleted successfully."
                    : "The product could not be found.";

            return RedirectToAction(
                nameof(Products)
            );
        }

        private async Task PopulateProductLookupsAsync()
        {
            ViewBag.Categories =
                await _productService
                    .GetCategoriesAsync();

            ViewBag.SubCategories =
                await _productService
                    .GetSubCategoriesAsync();
        }

        private async Task ValidateProductSelectionAsync(
            AdminProductViewModel model)
        {
            var categories =
                await _productService
                    .GetCategoriesAsync();

            var subCategories =
                await _productService
                    .GetSubCategoriesAsync();

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
        public async Task<IActionResult> Reviews()
        {
            var reviews =
                await _reviewService
                    .GetReviewsAsync();

            ViewBag.Branches =
                await _branchService
                    .GetBranchesAsync();

            return View(reviews);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteReview(
            int id)
        {
            var deleted =
                await _reviewService
                    .DeleteReviewAsync(id);

            TempData["ReviewMessage"] =
                deleted
                    ? "Review deleted successfully."
                    : "The review could not be found.";

            return RedirectToAction(
                nameof(Reviews)
            );
        }
    }
}
