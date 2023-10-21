using Shopping.Models.DTO;
using Shopping.Repositories.Infrastructure;
using static Shopping.Models.Domain.DatabaseContexct;

namespace Shopping.Repositories.Services
{
    public class AttributeService : GenericRepository<AttributeMasterModel> ,IAttributeService
    {
        private readonly DatabaseContext _context;
        public AttributeService(DatabaseContext context) : base(context)
        {
            _context = context;
        }
    }
}
