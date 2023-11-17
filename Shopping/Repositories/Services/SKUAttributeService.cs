using Shopping.Models.DTO;
using Shopping.Repositories.Infrastructure;
using static Shopping.Models.Domain.DatabaseContexct;

namespace Shopping.Repositories.Services
{
    public class SKUAttributeService : GenericRepository<SKUAttributeModel>, ISKUAttributeService
    {
        private readonly DatabaseContext _context;

        public SKUAttributeService(DatabaseContext context) : base(context)
        {
            _context = context;
        }
    }
}
