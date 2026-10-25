using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QPET.Models;
using QPET.Services.Interfaces;
using System.Security.Claims;
using QPET.Data.Entities;

namespace QPET.Controllers
{
    [Authorize(
        AuthenticationSchemes = "QPETAdmin",
        Roles = "Admin"
    )]
    public class AdminController : Controller
    {
        private readonly IAdminAuthService _adminAuthService;
        private readonly IProductService _productService;
        private readonly IBranchService _branchService;
        private readonly IEnquiryService _enquiryService;
        private readonly IReviewService _reviewService;
        private readonly IEnquiryFileStorageService _enquiryFileStorageService;
        private readonly IWebHostEnvironment _environment;

        public AdminController(
    IAdminAuthService adminAuthService,
    IProductService productService,
    IBranchService branchService,
    IEnquiryService enquiryService,
    IReviewService reviewService,
    IEnquiryFileStorageService enquiryFileStorageService,
    IWebHostEnvironment environment)
        {
            _adminAuthService = adminAuthService;
            _productService = productService;
            _branchService = branchService;
            _enquiryService = enquiryService;
            _reviewService = reviewService;
            _enquiryFileStorageService = enquiryFileStorageService;
            _environment = environment;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login(
            string? returnUrl = null)
        {
            if (
                User.Identity?.IsAuthenticated ==
                true)
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
                await _adminAuthService
                    .AuthenticateAsync(
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
                        ClaimTypes.NameIdentifier,
                        admin.AdminUserId.ToString()
                    ),

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

        [HttpGet]
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
                                        .ToString(
                                            "dd MMM yyyy"
                                        ),

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


        [HttpGet]
        public async Task<IActionResult> Enquiries(
            string? status = null,
            string? search = null,
            int? branchId = null,
            DateTime? fromDate = null,
            DateTime? toDate = null)
        {
            var allowedStatuses = new[]
            {
        "New",
        "InProgress",
        "Resolved",
        "Closed"
    };

            if (!string.IsNullOrWhiteSpace(status) &&
                !allowedStatuses.Contains(status))
            {
                status = null;
            }

            search = search?.Trim();

            if (string.IsNullOrWhiteSpace(search))
            {
                search = null;
            }

            if (branchId <= 0)
            {
                branchId = null;
            }

            var enquiries = await _enquiryService
                .GetEnquiriesAsync();

            var branches = await _branchService
                .GetBranchesAsync();

            var customers = await _enquiryService
                .GetCustomersAsync();

            IEnumerable<QPET.Data.Entities.Enquiry> filtered = enquiries;

            if (!string.IsNullOrWhiteSpace(status))
            {
                filtered = filtered.Where(
                    enquiry => enquiry.Status == status);
            }

            if (branchId.HasValue)
            {
                filtered = filtered.Where(
                    enquiry => enquiry.BranchId == branchId.Value);
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                var idSearch = search.StartsWith(
                    "ENQ-",
                    StringComparison.OrdinalIgnoreCase)
                    ? search.Substring(4).Trim()
                    : search;

                var isId = int.TryParse(
                    idSearch,
                    out var searchedId);

                filtered = filtered.Where(enquiry =>
                    (isId && enquiry.EnquiryId == searchedId) ||
                    enquiry.Customer.FullName.Contains(
                        search,
                        StringComparison.OrdinalIgnoreCase) ||
                    enquiry.Customer.EmailAddress.Contains(
                        search,
                        StringComparison.OrdinalIgnoreCase) ||
                    enquiry.Subject.Contains(
                        search,
                        StringComparison.OrdinalIgnoreCase));
            }

            string? filterError = null;

            if (fromDate.HasValue &&
                toDate.HasValue &&
                fromDate.Value.Date > toDate.Value.Date)
            {
                filterError =
                    "The start date cannot be later than the end date.";

                filtered = Enumerable.Empty<QPET.Data.Entities.Enquiry>();
            }
            else
            {
                if (fromDate.HasValue)
                {
                    filtered = filtered.Where(enquiry =>
                        enquiry.CreatedDate.Date >= fromDate.Value.Date);
                }

                if (toDate.HasValue)
                {
                    filtered = filtered.Where(enquiry =>
                        enquiry.CreatedDate.Date <= toDate.Value.Date);
                }
            }

            ViewBag.Customers = customers;
            ViewBag.Branches = branches;

            var model = new AdminEnquiryListViewModel
            {
                Enquiries = filtered.ToList(),
                TotalCount = enquiries.Count,
                Search = search,
                SelectedStatus = status,
                SelectedBranchId = branchId,
                FromDate = fromDate,
                ToDate = toDate,
                FilterError = filterError
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
                ViewBag.EnquiryNotFound =
                    true;

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

        [HttpGet]
        public async Task<IActionResult>
            DownloadEnquiryAttachment(
                int id)
        {
            var attachment =
                await _enquiryService
                    .GetAttachmentByIdAsync(id);

            if (attachment == null)
            {
                return NotFound();
            }

            var filePath =
                _enquiryFileStorageService
                    .GetFilePath(
                        attachment.StoredFileName
                    );

            if (filePath == null)
            {
                return NotFound();
            }

            var contentType =
                string.IsNullOrWhiteSpace(
                    attachment.ContentType
                ) ||
                attachment.ContentType ==
                "unknown"
                    ? "application/octet-stream"
                    : attachment.ContentType;

            return PhysicalFile(
                filePath,
                contentType,
                attachment.OriginalFileName
            );
        }


        [HttpGet]
        public async Task<IActionResult> Products(
            string? search = null,
            int? categoryId = null,
            int? subCategoryId = null,
            string? capacity = null,
            string? neckFinish = null,
            string? status = null)
        {
            var products = await _productService.GetProductsAsync();
            var categories = await _productService.GetCategoriesAsync();
            var subCategories = await _productService.GetSubCategoriesAsync();

            search = search?.Trim();
            capacity = capacity?.Trim();
            neckFinish = neckFinish?.Trim();
            status = status?.Trim().ToLowerInvariant();

            if (status != "active" && status != "inactive")
            {
                status = null;
            }

            if (categoryId <= 0)
            {
                categoryId = null;
            }

            if (subCategoryId <= 0)
            {
                subCategoryId = null;
            }

            IEnumerable<QPET.Data.Entities.Product> filteredProducts = products;

            if (!string.IsNullOrWhiteSpace(search))
            {
                filteredProducts = filteredProducts.Where(product =>
                    product.ProductName.Contains(
                        search, StringComparison.OrdinalIgnoreCase) ||
                    product.Description.Contains(
                        search, StringComparison.OrdinalIgnoreCase) ||
                    product.NeckFinish.Contains(
                        search, StringComparison.OrdinalIgnoreCase) ||
                    product.CapacityVolume.Contains(
                        search, StringComparison.OrdinalIgnoreCase) ||
                    product.ProductId.ToString() == search);
            }

            if (categoryId.HasValue)
            {
                filteredProducts = filteredProducts.Where(product =>
                    product.CategoryId == categoryId.Value);
            }

            if (subCategoryId.HasValue)
            {
                filteredProducts = filteredProducts.Where(product =>
                    product.SubCategoryId == subCategoryId.Value);
            }

            if (!string.IsNullOrWhiteSpace(capacity))
            {
                filteredProducts = filteredProducts.Where(product =>
                    string.Equals(
                        product.CapacityVolume.Trim(),
                        capacity,
                        StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(neckFinish))
            {
                filteredProducts = filteredProducts.Where(product =>
                    string.Equals(
                        product.NeckFinish.Trim(),
                        neckFinish,
                        StringComparison.OrdinalIgnoreCase));
            }

            if (status == "active")
            {
                filteredProducts = filteredProducts.Where(product =>
                    product.IsActive);
            }
            else if (status == "inactive")
            {
                filteredProducts = filteredProducts.Where(product =>
                    !product.IsActive);
            }

            var model = new ProductCatalogueViewModel
            {
                Products = filteredProducts
                    .OrderBy(product => product.ProductName)
                    .ToList(),

                Categories = categories,
                SubCategories = subCategories,

                Capacities = products
                    .Select(product => product.CapacityVolume.Trim())
                    .Where(value => !string.IsNullOrWhiteSpace(value))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .OrderBy(value => value)
                    .ToList(),

                NeckFinishes = products
                    .Select(product => product.NeckFinish.Trim())
                    .Where(value => !string.IsNullOrWhiteSpace(value))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .OrderBy(value => value)
                    .ToList(),

                Search = search,
                SelectedCategoryId = categoryId,
                SelectedSubCategoryId = subCategoryId,
                SelectedCapacity = capacity,
                SelectedNeckFinish = neckFinish,
                SelectedStatus = status,

                TotalProductCount = products.Count,
                ActiveProductCount = products.Count(product => product.IsActive),
                InactiveProductCount = products.Count(product => !product.IsActive)
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

            string? imageUrl = null;

            if (model.Image != null)
            {
                imageUrl =
                    await SaveProductImageAsync(
                        model.Image
                    );
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

            if (!string.IsNullOrWhiteSpace(imageUrl))
            {
                product.Images.Add(
                    new QPET.Data.Entities.ProductImage
                    {
                        ImageUrl = imageUrl,
                        IsPrimary = true
                        }
                    );
            }

            try
            {
                var createdProduct =
                    await _productService
                        .AddProductAsync(product);

                TempData["ProductMessage"] =
                    $"Product created successfully. Product ID: {createdProduct.ProductId}";

                return RedirectToAction(
                    nameof(Products)
                );
            }
            catch
            {
                if (!string.IsNullOrWhiteSpace(imageUrl))
                {
                    DeleteProductImageFile(imageUrl);
                }

                throw;
            }
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

            model.ProductId = id;

            await ValidateProductSelectionAsync(
                model
            );

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

            string? newImageUrl = null;

            if (model.Image != null)
            {
                newImageUrl =
                    await SaveProductImageAsync(
                        model.Image
                    );
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

            if (!string.IsNullOrWhiteSpace(newImageUrl))
            {
                updatedProduct.Images.Add(
                    new QPET.Data.Entities.ProductImage
                    {
                        ImageUrl = newImageUrl,
                        IsPrimary = true
                    }
                );
            }

            var oldImageUrl =
                existingProduct
                    .Images
                    .FirstOrDefault(
                        image =>
                            image.IsPrimary
                    )
                    ?.ImageUrl;

            try
            {
                var updated =
                    await _productService
                        .UpdateProductAsync(
                            updatedProduct
                        );

                if (!updated)
                {
                    if (!string.IsNullOrWhiteSpace(newImageUrl))
                    {
                        DeleteProductImageFile(
                            newImageUrl
                        );
                    }

                    TempData["ProductMessage"] =
                        "The product could not be updated.";

                    return RedirectToAction(
                        nameof(Products)
                    );
                }

                if (
                    !string.IsNullOrWhiteSpace(
                        newImageUrl
                    ) &&
                    !string.IsNullOrWhiteSpace(
                        oldImageUrl
                    )
                )
                {
                    DeleteProductImageFile(
                        oldImageUrl
                    );
                }

                TempData["ProductMessage"] =
                    "Product updated successfully.";

                return RedirectToAction(
                    nameof(Products)
                );
            }
            catch
            {
                if (!string.IsNullOrWhiteSpace(newImageUrl))
                {
                    DeleteProductImageFile(
                        newImageUrl
                    );
                }

                throw;
            }
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

        [HttpGet]
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
        private async Task<string> SaveProductImageAsync(
    IFormFile image)
        {
            var imageDirectory =
                Path.Combine(
                    _environment.WebRootPath,
                    "media",
                    "images"
                );

            Directory.CreateDirectory(
                imageDirectory
            );

            var extension =
                Path.GetExtension(
                    image.FileName
                )
                .ToLowerInvariant();

            var fileName =
                $"{Guid.NewGuid():N}{extension}";

            var physicalPath =
                Path.Combine(
                    imageDirectory,
                    fileName
                );

            await using (
                var stream =
                    new FileStream(
                        physicalPath,
                        FileMode.CreateNew
                    ))
            {
                await image.CopyToAsync(stream);
            }

            return $"/media/images/{fileName}";
        }

        private void DeleteProductImageFile(
     string imageUrl)
        {
            if (
                string.IsNullOrWhiteSpace(
                    imageUrl
                )
            )
            {
                return;
            }

            if (
                !imageUrl.StartsWith(
                    "/media/images/",
                    StringComparison.OrdinalIgnoreCase
                )
            )
            {
                return;
            }

            var fileName =
                Path.GetFileName(
                    imageUrl
                );

            if (
                string.IsNullOrWhiteSpace(
                    fileName
                )
            )
            {
                return;
            }

            var imageDirectory =
                Path.Combine(
                    _environment.WebRootPath,
                    "media",
                    "images"
                );

            var physicalPath =
                Path.Combine(
                    imageDirectory,
                    fileName
                );

            if (
                System.IO.File.Exists(
                    physicalPath
                )
            )
            {
                System.IO.File.Delete(
                    physicalPath
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