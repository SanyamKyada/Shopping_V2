using Shopping.Models.DTO;

namespace Shopping.Repositories.Infrastructure
{
    public interface ISearchService
    {
        List<ProductModel> GetMatchingProductsBySearchQuery(string searchQuery, int n, int tolerance);

        List<SKUModel> GetSKUsOfMatchingProducts(List<ProductModel> matchingProducts);
    }
}
