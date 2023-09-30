using Shopping.Models.Domain;
using Shopping.Models.DTO;
using Shopping.Models.ViewModels;
using Shopping.Repositories.Infrastructure;
using static Shopping.Models.Domain.DatabaseContexct;

namespace Shopping.Repositories.Services
{
	public class CartService : ICartService
	{
		private readonly DatabaseContext  _context;
		public CartService(DatabaseContext context)
		{
			_context = context;
		}
		public void AddToCart(int skuId, int quantity, Guid userId)
		{
            var cartItem = _context.CartItems.FirstOrDefault(x => x.UserId == userId && x.SKUId == skuId);

            if (cartItem == null)
            {
                //var prdctImg = _context.ProductImages.FirstOrDefault(i => i.ProductId == id && i.IsMain)?.ImageUrl;

                //var productToBeAdded = _context.Products.Where(x => x.Id == id).FirstOrDefault();

                var model = new CartModel()
                {
                    SKUId = skuId,
                    //Name = productToBeAdded.Name,
                    //Price = productToBeAdded.SellingPrice,
                    //ImageUrl = prdctImg,
                    Quantity = quantity,
                    UserId = userId
                };

                _context.CartItems.Add(model);
            }
            else
            {
                cartItem.Quantity += quantity;
                Update(cartItem);
            }
            _context.SaveChanges();
        }

		public List<Vm_CartItems> GetAllByUserId(Guid userId)
		{
            var cartItems = _context.CartItems.Where(ci => ci.UserId == userId).ToList();

            var vm_CartItems = new List<Vm_CartItems>();
            var skuIds = cartItems.Select(i => i.SKUId).ToList();
            var skus = _context.SKUs.Where(s => skuIds.Contains(s.Id)).ToList();
            var cmnSkuIds = skus.Select(s => s.CommonSkuId).ToList();
            var images = _context.ProductImages.Where(s => cmnSkuIds.Contains(s.CmnSkuId)).ToList();

            foreach (var cartItem in cartItems)
            {
                var cmnSkuId = skus.FirstOrDefault(p => p.Id == cartItem.SKUId).CommonSkuId;
                var model = new Vm_CartItems()
                {
                    Quantity = cartItem.Quantity,
                    Price = skus.FirstOrDefault(p => p.Id == cartItem.SKUId).SellingPrice,
                    Name = skus.FirstOrDefault(p => p.Id == cartItem.SKUId).Title,
                    ImageUrl = images.FirstOrDefault(p => p.CmnSkuId== cmnSkuId).ImageUrl
                };
                vm_CartItems.Add(model);
            }
            
            return vm_CartItems;
		}
       /* public CartModel GetById(int id)
        {
            var prdctImg = _context.ProductImages.FirstOrDefault(i => i.ProductId == id && i.IsMain)?.ImageUrl;

            var productToBeAdded = _context.Products.Where(x => x.Id == id).FirstOrDefault();

            var model = new CartModel()
            {
                Name = productToBeAdded.Name,
                Price = productToBeAdded.SellingPrice,
                ImageUrl = prdctImg,
                Quantity = quantity
            };
            return model;
        }*/
        public void Update(CartModel cartprdt)
        {
            _context.Update(cartprdt);
        }
        public void Delete(int id)
        {
            var cartprdt = _context.CartItems.FirstOrDefault(i => i.Id == id);
            _context.Remove(cartprdt);
        }
		public void Save()
		{
			_context.SaveChanges();
		}
	}
}
