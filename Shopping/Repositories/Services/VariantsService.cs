using Shopping.Models.DTO;
using Shopping.Repositories.Infrastructure;
using static Shopping.Models.Domain.DatabaseContexct;

namespace Shopping.Repositories.Services
{
    public class VariantsService : GenericRepository<SKUModel>, IVariantsService
    {
        private readonly DatabaseContext _context;

        public VariantsService(DatabaseContext context) : base(context)
        {
            _context = context;
        }

        public int GetMaxCommonSKUId()
        {
            return _context.SKUs.Max(column => column.CommonSkuId);
        }
    }
}
