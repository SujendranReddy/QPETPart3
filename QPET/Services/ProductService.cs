using Microsoft.EntityFrameworkCore;
using QPET.Data;
using QPET.Data.Entities;
using QPET.Services.Interfaces;

namespace QPET.Services
{
    public class ProductService : IProductService
    {
        private readonly QpetDbContext _context;

        public ProductService(QpetDbContext context)
        {
            _context = context;
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

            if (!product.Images.Any(
                    image =>
                        image.IsPrimary))
            {
                product.Images
                    .First()
                    .IsPrimary = true;
            }

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
                    .FirstOrDefaultAsync(
                        product =>
                            product.ProductId ==
                            productId
                    );

            if (product == null)
            {
                return false;
            }

            _context.Products.Remove(product);

            await _context.SaveChangesAsync();

            return true;
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
