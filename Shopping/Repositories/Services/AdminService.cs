using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Shopping.Models.Domain;
using Shopping.Models.DTO;
using Shopping.Repositories.Infrastructure;
using static Shopping.Models.Domain.DatabaseContexct;

namespace Shopping.Repositories.Services
{
    public class AdminService<T> : IAdminService<T> where T : class
    {   
        private readonly DatabaseContext _context;
        private readonly DbSet<T> entities;
        public AdminService(DatabaseContext context)
        {
            _context = context;
            entities = _context.Set<T>();
        }
        public IEnumerable<T> GetAll()
        {
            return entities.AsEnumerable();
        }

        public T GetById(int id)
        {
            return entities.Find(id); // We can also use -- Where(x => x.Id == id).FirstOrDefault() -- Instead of Find()
        }

        public void Delete(T t)
        {
            entities.Remove(t);
        }

        public void Insert(T t)
        {
            entities.Add(t);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(T t)
        {
            entities.Update(t);
        }
    }
}
