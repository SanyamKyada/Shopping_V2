using Shopping.Models.DTO;
using Shopping.Models.ViewModels;

namespace Shopping.Repositories.Infrastructure
{
    public interface IProductService : IGenericRepository<ProductModel>
    {
        Task<List<ProductModel>> GetPaginatedProductsAsync(int categoryId, int currentPage, int pageSize);

        Task<int> GetTotalPageCountAsync(int categoryId, int pageSize);

        Task<string> GetUserNameAsync(string userN);

        Task<Vm_ProductDetail> GetProductDetailsAsync(int id);

        Task AddProductReviewAsync(int productId, string userName, string reviewText, int rating, int parentId);

        List<ProductColourModel> GetProductColours();

    }
}
