using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shopping.Models.DTO;

namespace Shopping.Models.Domain
{
    public class DatabaseContexct
    {
        public class DatabaseContext : IdentityDbContext<ApplicationUser>
        {
            public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
            {

            }
            public DbSet<ProductModel> Products { get; set; }
            public DbSet<CartModel> CartItems { get; set; }
            public DbSet<MenuItemsModelNew> MenuTableNew { get; set; }
            public DbSet<ProductImage> ProductImages { get; set; }
            public DbSet<AttributeMasterModel> AttributeMaster { get; set; }
            public DbSet<SKUAttributeModel> SKUAttributes { get; set; }
            public DbSet<SKUModel> SKUs { get; set; }
            public DbSet<FeaturedProductsModel> FeaturedProducts { get; set; }
            public DbSet<BannerModel> BannerProducts { get; set; }
            public DbSet<ProductAttributeModel> ProductAttributes { get; set; }
            public DbSet<ProductReviewModel> ProductReviews { get; set; }

        }
    }
}
