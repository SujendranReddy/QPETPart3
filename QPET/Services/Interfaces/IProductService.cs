
using QPET.Data.Entities;

namespace QPET.Services.Interfaces
{
    public interface IProductService
    {
        Task<List<Product>> GetProductsAsync(
            bool activeOnly = false);

        Task<Product?> GetProductByIdAsync(int productId);

        Task<List<Category>> GetCategoriesAsync();

        Task<List<SubCategory>> GetSubCategoriesAsync();

        Task<Product> AddProductAsync(Product product);

        Task<bool> UpdateProductAsync(Product product);

        Task<bool> DeleteProductAsync(int productId);

        Task<(bool Success, string Message)> CreateCategoryAsync(
            string? name,
            string? description);

        Task<(bool Success, string Message)> UpdateCategoryAsync(
            int id,
            string? name,
            string? description);

        Task<(bool Success, string Message)> DeleteCategoryAsync(
            int id);

        Task<(bool Success, string Message)> CreateSubCategoryAsync(
            int categoryId,
            string? name);

        Task<(bool Success, string Message)> UpdateSubCategoryAsync(
            int id,
            int categoryId,
            string? name);

        Task<(bool Success, string Message)> DeleteSubCategoryAsync(
            int id);
    }
}
