using Shopping.Models.DTO;

namespace Shopping.Repositories.Infrastructure
{
    public interface IAdminService<T> where T : class
    {
        IEnumerable<T> GetAll();
        T GetById(int id);
        void Insert(T t);
        void Update(T t);
        void Delete(T t);
        void Save();
    }
}
