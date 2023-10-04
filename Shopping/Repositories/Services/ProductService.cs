using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Shopping.Models.DTO;
using Shopping.Models.ViewModels;
using Shopping.Repositories.Infrastructure;
using System.Diagnostics;
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
            /// Implemented: asynchronous database query to get the total product
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

        public async Task<Vm_ProductDetail> GetProductDetailsAsync(int id)
        {
            /// Implemented: asynchronous database queries to retrieve product details, SKUs, reviews, etc.
            // Query:
            var sku = await _context.SKUs.FirstOrDefaultAsync(s => s.Id == id);
            var product = await _context.Products.FirstOrDefaultAsync(s => s.Id == sku.ProductId);
            var model = new Vm_ProductDetail()
            {
                _SKU = sku,
                _Product = product,
                _SKUList = await _context.SKUs
                    .Include(s => s.SKUAttributeModels)
                    .Where(sl => sl.ProductId == sku.ProductId)
                    .ToListAsync(),
                skuAttrs = await _context.SKUAttributes
                    .Where(sa => sa.SKUId == id)
                    .ToListAsync(),
                attrMstrs = await _context.AttributeMaster
                    .Where(pi => pi.CategoryId == product.CategoryId)
                    .ToListAsync(),
                SKUImages = await _context.ProductImages
                    .Where(i => i.CmnSkuId == sku.CommonSkuId)
                    .ToListAsync(),
                ReviewModels = await _context.ProductReviews
                    .Where(i => i.ProductId == sku.ProductId)
                    .ToListAsync(),
                relatedSKUList = await _context.SKUs
                    .Where(sku => sku.Product.CategoryId == product.CategoryId && sku.Id != id)
                    .OrderBy(sku => Guid.NewGuid())
                    .Take(6)
                    .ToListAsync()
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

    }
}