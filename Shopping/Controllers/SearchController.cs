using Microsoft.AspNetCore.Mvc;
using Shopping.Models.DTO;
using static Shopping.Models.Domain.DatabaseContexct;

namespace Shopping.Controllers
{
    public class SearchViewModel 
    {
        public List<ProductModel> Products { get; set; }    
        public List<SKUModel> SKU { get; set; }
        public string Query { get; set; }
    }

    public class SearchController : Controller
    {
        private readonly DatabaseContext _context;
        public SearchController(DatabaseContext context)
        {
            _context = context;
        }
        [HttpPost]
        public IActionResult Index(string query)
        {
            var matchingProducts = Search(query, 3, 2);
            ViewData["Query"] = query;

            var model = new SearchViewModel()
            {
                Products = matchingProducts,
                SKU = _context.SKUs.ToList(),
                Query = query
            };
            return View(model);
        }

        public List<ProductModel> Search(string query, int n, int tolerance)
        {
            var matches = new List<ProductModel>();
            foreach (var target in _context.Products.ToList())
            {
                var distance = LevenshteinDistance(query.ToUpper(), target.Name.ToUpper());
                if (distance <= tolerance)
                {
                    matches.Add(target);
                    continue;
                }

                var queryNGrams = GetNGrams(query, n);
                var targetNGrams = GetNGrams(target.Name, n);
                var similarity = GetSimilarity(queryNGrams, targetNGrams);
                if (similarity >= (double)(n - tolerance) / (double)n)
                {
                    matches.Add(target);
                }

                var searchWords = query.ToUpper().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                var targetWords = _context.SKUs.FirstOrDefault(i=>i.ProductId == target.Id).Description.ToUpper().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                if (targetWords.Intersect(searchWords).Count() == searchWords.Count())
                {
                    matches.Add(target);
                }
            }
            if (matches.Count() > 0)
            {
                return matches;
            }
            else
            {
                return null;
            }
        }

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
    }
}
