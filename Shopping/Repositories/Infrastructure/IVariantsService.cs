using Shopping.Models.DTO;

namespace Shopping.Repositories.Infrastructure
{
    public interface IVariantsService : IGenericRepository<SKUModel>
    {
        int GetMaxCommonSKUId();
    }
}
