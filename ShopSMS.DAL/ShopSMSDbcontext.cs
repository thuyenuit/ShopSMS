using Microsoft.AspNet.Identity.EntityFramework;
using ShopSMS.Model.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopSMS.DAL
{
    public class ShopSMSDbcontext: IdentityDbContext<ApplicationUser>
    {
        public ShopSMSDbcontext()
            : base("ShopSMSConnection")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<Footer> Footer { get; set; }
        public DbSet<Menu> Menu { get; set; }
        public DbSet<MenuGroup> MenuGroup { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderDetail> OrderDetail { get; set; }
        public DbSet<Post> Post { get; set; }
        public DbSet<PostCategory> PostCategory { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<ProductCategory> ProductCategory { get; set; }
        public DbSet<ProductTag> ProductTag { get; set; }
        public DbSet<Slide> Slide { get; set; }
        public DbSet<SystemConfig> SystemConfig { get; set; }
        public DbSet<Tag> Tag { get; set; }
        public DbSet<ErrorLog> ErrorLog { get; set; }

        public static ShopSMSDbcontext Create()
        {
            return new ShopSMSDbcontext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<IdentityUserRole>().HasKey(i => new { i.UserId, i.RoleId });
            modelBuilder.Entity<IdentityUserLogin>().HasKey(i => i.UserId);
            // create index
            modelBuilder.Entity<Product>()
                .HasIndex("Index_Product", IndexOptions.Unique, e => e.Property(x => x.ProductID));
            modelBuilder.Entity<ProductCategory>()
                .HasIndex("Index_ProductCategory", IndexOptions.Unique, e => e.Property(x => x.ProductCategoryID));

        }
    }
}
