using QPET.Data.Entities;

namespace QPET.Services.Interfaces
{
    public interface IProductService
    {
        Task<List<Product>> GetProductsAsync(bool activeOnly = false);

        Task<Product?> GetProductByIdAsync(int productId);

        Task<List<Category>> GetCategoriesAsync();

        Task<List<SubCategory>> GetSubCategoriesAsync();

        Task<Product> AddProductAsync(Product product);

        Task<bool> UpdateProductAsync(Product product);

        Task<bool> DeleteProductAsync(int productId);
    }
}
