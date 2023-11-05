using Shopping.Models.DTO;
using Shopping.Repositories.Infrastructure;
using static Shopping.Models.Domain.DatabaseContexct;

namespace Shopping.Repositories.Services
{
    public class ProductImageService : GenericRepository<ProductImage>, IProductImageService
    {
        private readonly DatabaseContext _context;

        public ProductImageService(DatabaseContext context) : base(context)
        {
            _context = context;
        }
    }
}
