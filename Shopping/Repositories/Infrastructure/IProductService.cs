using Shopping.Models.DTO;

namespace Shopping.Repositories.Infrastructure
{
    public interface IProductService 
    {
        Task<List<ProductModel>> GetPaginatedProductsAsync(int categoryId, int currentPage, int pageSize);

        Task<int> GetTotalPageCountAsync(int categoryId, int pageSize);

        Task<string> GetUserNameAsync(string userN);

        Task<SKUViewModel> GetProductDetailsAsync(int id);

        Task AddProductReviewAsync(int productId, string userName, string reviewText, int rating, int parentId);

    }
}
