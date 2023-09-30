using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Shopping.Models.DTO;
using Shopping.Repositories.Infrastructure;
using static Shopping.Models.Domain.DatabaseContexct;

namespace Shopping.Repositories.Services
{
    public class ProductService : IProductService /*, GenericRepository<SKUModel>*/
    {
        private readonly DatabaseContext _context;

        public ProductService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<List<ProductModel>> GetPaginatedProductsAsync(int categoryId, int currentPage, int pageSize)
        {
            /// Implemented: asynchronous database query to retrieve paginated products Including their SKUs
            // Query:
            var products = await _context.Products
                .Include(p => p.SKUs)
                .Where(p => p.CategoryId == categoryId)
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return products;
        }

        public async Task<int> GetTotalPageCountAsync(int categoryId, int pageSize)
        {
            /// Implemented asynchronous database query to get the total product
            // Query:
            var totalRecords = await _context.Products
                .Where(i => i.CategoryId == categoryId)
                .CountAsync();

            /// Calculation: To retrive the total number of pages needs to be shown
            return (int)Math.Ceiling((double)(Convert.ToDecimal(totalRecords) / Convert.ToDecimal(pageSize)));
        }

        public async Task<string> GetUserNameAsync(string userN)
        {
            /// Implemented asynchronous database query to get the user's name
            // Query:
            var user = await _context.Users
                .Where(n => n.UserName == userN)
                .Select(u => u.Name)
                .FirstOrDefaultAsync();

            return user;
        }

        public async Task<SKUViewModel> GetProductDetailsAsync(int id)
        {
            /// Implemented: asynchronous database queries to retrieve product details, SKUs, reviews, etc.
            // Query:
            var sku = await _context.SKUs.FindAsync(id)
;
            var product = await _context.Products.FindAsync(sku.ProductId);
            var skuImages = await _context.ProductImages.Where(i => i.CmnSkuId == sku.CommonSkuId).ToListAsync();

            var model = new SKUViewModel()
            {
                _SKU = sku,
                _SKUList = await _context.SKUs.Include(s => s.SKUAttributeModels).Where(sl => sl.ProductId == sku.ProductId).ToListAsync(),
                _Product = product,
                skuAttrs = await _context.SKUAttributes.Where(sa => sa.SKUId == id).ToListAsync(),
                attrMstrs = await _context.AttributeMaster.Where(pi => pi.CategoryId == product.CategoryId).ToListAsync(),
                SKUImages = skuImages,
                ReviewModels = await _context.ProductReviews.Where(i => i.ProductId == sku.ProductId).ToListAsync(),
                relatedSKUList = await GetRelatedSKUsByCategoryAsync(sku)
            };

            return model;
        }

        public async Task AddProductReviewAsync(int productId, string userName, string reviewText, int rating, int parentId)
        {
            /// Implemented: asynchronous database operations to add a product review
            // Query:
            var review = new ProductReviewModel
            {
                ProductId = productId,
                User = userName,
                Text = reviewText,
                Stars = rating,
                ParentReviewId = parentId
            };

            _context.ProductReviews.Add(review);
            await _context.SaveChangesAsync();
        }

        public async Task<List<SKUModel>> GetRelatedSKUsByCategoryAsync(SKUModel currentSku)
        {
            /// Implemented asynchronous database query to retrieve related SKUs by Category
            // Query:
            var prdtId = await _context.SKUs
                .Where(i => i.Id == currentSku.Id)
                .Select(sku => sku.ProductId)
                .FirstOrDefaultAsync();

            var prdt = await _context.Products
                .FirstOrDefaultAsync(i => i.Id == prdtId);

            var relatedSKUs = new List<SKUModel>();
            var prds = await _context.Products
                .Where(p => p.CategoryId == prdt.CategoryId && p.Id != prdt.Id)
                .OrderBy(x => Guid.NewGuid())
                .Take(6)
                .ToListAsync();

            foreach (var item in prds)
            {
                var sku = await _context.SKUs
                    .FirstOrDefaultAsync(i => i.ProductId == item.Id);
                relatedSKUs.Add(sku);
            }

            return relatedSKUs;

        }

    }
}