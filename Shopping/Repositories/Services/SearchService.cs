using Shopping.Models.DTO;
using Shopping.Repositories.Infrastructure;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Shopping.Models.Domain.DatabaseContexct;

namespace Shopping.Repositories.Services
{
    public class SearchService : ISearchService
    {
        private readonly DatabaseContext _context;

        public SearchService(DatabaseContext context)
        {
            _context = context;
        }

        public List<SKUModel> GetSKUsOfMatchingProducts(List<ProductModel> matchingProducts)
        {
            if (matchingProducts != null)
            {
                var productIds = matchingProducts
                .Select(prd => prd.Id)
                .ToList();

                if (productIds.Count > 0)
                {
                    return _context.SKUs.Where(sku => productIds.Contains(sku.ProductId)).ToList();
                }
            }

            return new List<SKUModel>();
        }
        public List<ProductModel> GetMatchingProductsBySearchQuery(string searchQuery, int n, int tolerance)
        {
            var matches = new List<ProductModel>();

            // Retrieve distinct SKUs where IsMain is true
            var distinctSkus = _context.SKUs
                .Where(sku => sku.IsMain == true)
                .Select(sku => new
                {
                    sku.ProductId,
                    sku.Description
                })
                .ToList();

            foreach (var target in _context.Products.ToList())
            {
                // Calculate Levenshtein distance
                var distance = LevenshteinDistance(searchQuery.ToUpper(), target.Name.ToUpper());
                if (distance <= tolerance)
                {
                    matches.Add(target);
                    continue;
                }

                // Calculate NGram similarity
                var queryNGrams = GetNGrams(searchQuery, n);
                var targetNGrams = GetNGrams(target.Name, n);
                var similarity = GetSimilarity(queryNGrams, targetNGrams);
                if (similarity >= (double)(n - tolerance) / (double)n)
                {
                    matches.Add(target);
                }

                // Check for word intersection
                var searchWords = searchQuery.ToUpper().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                var targetWords = distinctSkus.FirstOrDefault(i => i.ProductId == target.Id).Description.ToUpper().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                if (targetWords.Intersect(searchWords).Count() == searchWords.Count())
                {
                    matches.Add(target);
                }
            }

            return matches.Any() ? matches : null;
        }

        #region Private Methods
        private List<string> GetNGrams(string str, int n)
        {
            var ngrams = new List<string>();
            for (int i = 0; i < str.Length - n + 1; i++)
            {
                ngrams.Add(str.Substring(i, n));
            }
            return ngrams;
        }

        private int LevenshteinDistance(string s, string t)
        {
            int[,] d = new int[s.Length + 1, t.Length + 1];
            for (int i = 0; i <= s.Length; i++)
            {
                d[i, 0] = i;
            }
            for (int j = 0; j <= t.Length; j++)
            {
                d[0, j] = j;
            }
            for (int j = 1; j <= t.Length; j++)
            {
                for (int i = 1; i <= s.Length; i++)
                {
                    int cost = (s[i - 1] == t[j - 1]) ? 0 : 1;
                    d[i, j] = Math.Min(Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1), d[i - 1, j - 1] + cost);
                }
            }
            return d[s.Length, t.Length];
        }

        private double GetSimilarity(List<string> a, List<string> b)
        {
            var intersect = a.Intersect(b);
            var union = a.Union(b);
            return (double)intersect.Count() / (double)union.Count();
        }

        #endregion
    }
}
