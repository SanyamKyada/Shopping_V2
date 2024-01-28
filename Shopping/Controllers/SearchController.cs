using Microsoft.AspNetCore.Mvc;
using Shopping.Models.ViewModels;
using Shopping.Repositories.Infrastructure;

namespace Shopping.Controllers
{
    public class SearchController : Controller
    {
        private readonly ISearchService _searchService;
        public SearchController(ISearchService searchService)
        {
            _searchService = searchService;
        }
        [HttpPost]
        public IActionResult Index(string query)
        {
            var matchingProducts = _searchService.GetMatchingProductsBySearchQuery(query, 3, 2);

            var model = new Vm_SearchResults()
            {
                Products = matchingProducts,
                SKU = _searchService.GetSKUsOfMatchingProducts(matchingProducts),
                Query = query
            };
            return View(model);
        }
    }
}
