using Microsoft.EntityFrameworkCore;
using QPET.Data;
using QPET.Data.Entities;
using QPET.Services.Interfaces;

namespace QPET.Services
{
    public class ProductService : IProductService
    {
        private readonly QpetDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public ProductService(
            QpetDbContext context,
            IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<List<Product>> GetProductsAsync(
            bool activeOnly = false)
        {
            var query =
                _context.Products
                    .AsNoTracking()
                    .Include(product => product.Images)
                    .Include(product => product.Category)
                    .Include(product => product.SubCategory)
                    .AsQueryable();

            if (activeOnly)
            {
                query =
                    query.Where(
                        product =>
                            product.IsActive
                    );
            }

            return await query
                .OrderBy(
                    product =>
                        product.ProductName
                )
                .ToListAsync();
        }

        public async Task<Product?> GetProductByIdAsync(
            int productId)
        {
            if (productId <= 0)
            {
                return null;
            }

            return await _context.Products
                .AsNoTracking()
                .Include(product => product.Images)
                .Include(product => product.Category)
                .Include(product => product.SubCategory)
                .FirstOrDefaultAsync(
                    product =>
                        product.ProductId ==
                        productId
                );
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            return await _context.Categories
                .AsNoTracking()
                .OrderBy(
                    category =>
                        category.CategoryName
                )
                .ToListAsync();
        }

        public async Task<List<SubCategory>> GetSubCategoriesAsync()
        {
            return await _context.SubCategories
                .AsNoTracking()
                .OrderBy(
                    subCategory =>
                        subCategory.SubCategoryName
                )
                .ToListAsync();
        }


        public async Task<(bool Success, string Message)> CreateCategoryAsync(
            string? name,
            string? description)
        {
            name = name?.Trim();
            description = description?.Trim();

            if (string.IsNullOrWhiteSpace(name) ||
                name.Length > 100)
            {
                return (false,
                    "Enter a category name containing 1 to 100 characters.");
            }

            if (string.IsNullOrWhiteSpace(description) ||
                description.Length > 500)
            {
                return (false,
                    "Enter a category description containing 1 to 500 characters.");
            }

            var normalizedName = name.ToUpperInvariant();

            var duplicate = await _context.Categories.AnyAsync(
                category =>
                    category.CategoryName.ToUpper() == normalizedName);

            if (duplicate)
            {
                return (false, "A category with this name already exists.");
            }

            var category = new Category
            {
                CategoryName = name,
                Description = description
            };

            _context.Categories.Add(category);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex) when (
                ex.InnerException is Microsoft.Data.SqlClient.SqlException
                { Number: 2601 or 2627 })
            {
                return (false, "A category with this name already exists.");
            }

            return (true, "Category created successfully.");
        }

        public async Task<(bool Success, string Message)> UpdateCategoryAsync(
            int id,
            string? name,
            string? description)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(item => item.CategoryId == id);

            if (category == null)
            {
                return (false, "Category not found.");
            }

            name = name?.Trim();
            description = description?.Trim();

            if (string.IsNullOrWhiteSpace(name) ||
                name.Length > 100)
            {
                return (false,
                    "Enter a category name containing 1 to 100 characters.");
            }

            if (string.IsNullOrWhiteSpace(description) ||
                description.Length > 500)
            {
                return (false,
                    "Enter a category description containing 1 to 500 characters.");
            }

            var normalizedName = name.ToUpperInvariant();

            var duplicate = await _context.Categories.AnyAsync(
                item =>
                    item.CategoryId != id &&
                    item.CategoryName.ToUpper() == normalizedName);

            if (duplicate)
            {
                return (false, "Another category already uses this name.");
            }

            category.CategoryName = name;
            category.Description = description;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex) when (
                ex.InnerException is Microsoft.Data.SqlClient.SqlException
                { Number: 2601 or 2627 })
            {
                return (false, "Another category already uses this name.");
            }

            return (true, "Category updated successfully.");
        }

        public async Task<(bool Success, string Message)> DeleteCategoryAsync(
            int id)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(item => item.CategoryId == id);

            if (category == null)
            {
                return (false, "Category not found.");
            }

            var hasSubCategories = await _context.SubCategories
                .AnyAsync(item => item.CategoryId == id);

            var hasProducts = await _context.Products
                .AnyAsync(item => item.CategoryId == id);

            if (hasSubCategories || hasProducts)
            {
                return (false,
                    "This category cannot be deleted because it contains subcategories or products.");
            }

            _context.Categories.Remove(category);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex) when (
                ex.InnerException is Microsoft.Data.SqlClient.SqlException
                { Number: 547 })
            {
                return (false,
                    "This category is still referenced by other records.");
            }

            return (true, "Category deleted successfully.");
        }

        public async Task<(bool Success, string Message)> CreateSubCategoryAsync(
            int categoryId,
            string? name)
        {
            name = name?.Trim();

            if (string.IsNullOrWhiteSpace(name) ||
                name.Length > 100)
            {
                return (false,
                    "Enter a subcategory name containing 1 to 100 characters.");
            }

            var categoryExists = await _context.Categories
                .AnyAsync(item => item.CategoryId == categoryId);

            if (!categoryExists)
            {
                return (false, "Select a valid parent category.");
            }

            var normalizedName = name.ToUpperInvariant();

            var duplicate = await _context.SubCategories.AnyAsync(
                item =>
                    item.CategoryId == categoryId &&
                    item.SubCategoryName.ToUpper() == normalizedName);

            if (duplicate)
            {
                return (false,
                    "A subcategory with this name already exists under the selected category.");
            }

            var subCategory = new SubCategory
            {
                CategoryId = categoryId,
                SubCategoryName = name
            };

            _context.SubCategories.Add(subCategory);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex) when (
                ex.InnerException is Microsoft.Data.SqlClient.SqlException
                { Number: 2601 or 2627 })
            {
                return (false,
                    "A subcategory with this name already exists under the selected category.");
            }
            catch (DbUpdateException ex) when (
                ex.InnerException is Microsoft.Data.SqlClient.SqlException
                { Number: 547 })
            {
                return (false, "The selected parent category is unavailable.");
            }

            return (true, "Subcategory created successfully.");
        }

        public async Task<(bool Success, string Message)> UpdateSubCategoryAsync(
            int id,
            int categoryId,
            string? name)
        {
            var subCategory = await _context.SubCategories
                .FirstOrDefaultAsync(item => item.SubCategoryId == id);

            if (subCategory == null)
            {
                return (false, "Subcategory not found.");
            }

            name = name?.Trim();

            if (string.IsNullOrWhiteSpace(name) ||
                name.Length > 100)
            {
                return (false,
                    "Enter a subcategory name containing 1 to 100 characters.");
            }

            var categoryExists = await _context.Categories
                .AnyAsync(item => item.CategoryId == categoryId);

            if (!categoryExists)
            {
                return (false, "Select a valid parent category.");
            }

            if (subCategory.CategoryId != categoryId)
            {
                var hasProducts = await _context.Products
                    .AnyAsync(item => item.SubCategoryId == id);

                if (hasProducts)
                {
                    return (false,
                        "This subcategory cannot be moved because products are still assigned to it.");
                }
            }

            var normalizedName = name.ToUpperInvariant();

            var duplicate = await _context.SubCategories.AnyAsync(
                item =>
                    item.SubCategoryId != id &&
                    item.CategoryId == categoryId &&
                    item.SubCategoryName.ToUpper() == normalizedName);

            if (duplicate)
            {
                return (false,
                    "Another subcategory under this category already uses that name.");
            }

            subCategory.CategoryId = categoryId;
            subCategory.SubCategoryName = name;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex) when (
                ex.InnerException is Microsoft.Data.SqlClient.SqlException
                { Number: 2601 or 2627 })
            {
                return (false,
                    "Another subcategory under this category already uses that name.");
            }
            catch (DbUpdateException ex) when (
                ex.InnerException is Microsoft.Data.SqlClient.SqlException
                { Number: 547 })
            {
                return (false,
                    "The selected parent category is unavailable or the subcategory is still referenced.");
            }

            return (true, "Subcategory updated successfully.");
        }

        public async Task<(bool Success, string Message)> DeleteSubCategoryAsync(
            int id)
        {
            var subCategory = await _context.SubCategories
                .FirstOrDefaultAsync(item => item.SubCategoryId == id);

            if (subCategory == null)
            {
                return (false, "Subcategory not found.");
            }

            var hasProducts = await _context.Products
                .AnyAsync(item => item.SubCategoryId == id);

            if (hasProducts)
            {
                return (false,
                    "This subcategory cannot be deleted because products are assigned to it.");
            }

            _context.SubCategories.Remove(subCategory);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex) when (
                ex.InnerException is Microsoft.Data.SqlClient.SqlException
                { Number: 547 })
            {
                return (false,
                    "This subcategory is still referenced by products.");
            }

            return (true, "Subcategory deleted successfully.");
        }

        public async Task<Product> AddProductAsync(
            Product product)
        {
            var validSelection =
                await IsValidProductSelectionAsync(
                    product.CategoryId,
                    product.SubCategoryId
                );

            if (!validSelection)
            {
                throw new ArgumentException(
                    "The selected category or subcategory is invalid."
                );
            }

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

            foreach (var image in product.Images)
            {
                image.IsPrimary = false;
            }

            product.Images.First().IsPrimary = true;

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return product;
        }

        public async Task<bool> UpdateProductAsync(
            Product product)
        {
            if (product.ProductId <= 0)
            {
                return false;
            }

            var existingProduct =
                await _context.Products
                    .Include(
                        existing =>
                            existing.Images
                    )
                    .FirstOrDefaultAsync(
                        existing =>
                            existing.ProductId ==
                            product.ProductId
                    );

            if (existingProduct == null)
            {
                return false;
            }

            var validSelection =
                await IsValidProductSelectionAsync(
                    product.CategoryId,
                    product.SubCategoryId
                );

            if (!validSelection)
            {
                return false;
            }

            existingProduct.CategoryId =
                product.CategoryId;

            existingProduct.SubCategoryId =
                product.SubCategoryId;

            existingProduct.ProductName =
                product.ProductName;

            existingProduct.NeckFinish =
                product.NeckFinish;

            existingProduct.CapacityVolume =
                product.CapacityVolume;

            existingProduct.Description =
                product.Description;

            existingProduct.IsActive =
                product.IsActive;

            if (product.Images.Any())
            {
                foreach (
                    var existingImage
                    in existingProduct.Images.ToList())
                {
                    _context.ProductImages.Remove(
                        existingImage
                    );
                }

                foreach (
                    var image
                    in product.Images)
                {
                    image.ProductId =
                        existingProduct.ProductId;

                    image.ProductImageId = 0;

                    _context.ProductImages.Add(
                        image
                    );
                }
            }
            else if (!existingProduct.Images.Any())
            {
                existingProduct.Images.Add(
                    new ProductImage
                    {
                        ImageUrl =
                            "/images/products/product-placeholder.jpg",
                        IsPrimary = true
                    }
                );
            }

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteProductAsync(
            int productId)
        {
            if (productId <= 0)
            {
                return false;
            }

            var product =
                await _context.Products
                    .Include(
                        current =>
                            current.Images
                    )
                    .FirstOrDefaultAsync(
                        product =>
                            product.ProductId ==
                            productId
                    );

            if (product == null)
            {
                return false;
            }

            var imageUrls =
                product.Images
                    .Select(
                        image =>
                            image.ImageUrl
                    )
                    .ToList();

            _context.Products.Remove(product);

            await _context.SaveChangesAsync();

            // Removes uploaded product image files after a product is deleted to prevent orphaned files.
            foreach (var imageUrl in imageUrls)
            {
                DeleteProductImageFile(
                    imageUrl
                );
            }

            return true;
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

        private async Task<bool> IsValidProductSelectionAsync(
            int categoryId,
            int subCategoryId)
        {
            return await _context.SubCategories
                .AnyAsync(
                    subCategory =>
                        subCategory.SubCategoryId ==
                        subCategoryId &&
                        subCategory.CategoryId ==
                        categoryId
                );
        }
    }
}