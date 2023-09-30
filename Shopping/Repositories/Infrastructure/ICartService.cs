using Shopping.Models.DTO;
using Shopping.Models.ViewModels;

namespace Shopping.Repositories.Infrastructure
{
    public interface ICartService
    {      
        public List<Vm_CartItems> GetAllByUserId(Guid userId);
        //public CartModel GetById(int id);
        void AddToCart(int id, int quantity, Guid userId);
        void Update(CartModel cartprdt);
        void Delete(int id);
        void Save();    

	}
}
