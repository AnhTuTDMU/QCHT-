using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using Website_InAnQuangCaoHTD.Models;

namespace Website_InAnQuangCaoHTD.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<FlashSale> FlashSales { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<UsersModel> Users { get; set; } 
        public DbSet<RolesModel> Roles { get; set; }
        public DbSet<ConstructionBooking> ConstructionBookings { get; set; }

        [HttpGet]
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Cấu hình quan hệ giữa UsersModel và RolesModel
            modelBuilder.Entity<UsersModel>(entity =>
            {
                entity.HasKey(e => e.UserId);
                entity.Property(e => e.UserName).IsRequired();
                entity.Property(e => e.UserEmail).IsRequired();
                entity.Property(e => e.Password).IsRequired();
                entity.Property(e => e.Address).IsRequired();
                entity.Property(e => e.PhoneNumber).IsRequired();

                entity.HasOne(e => e.Role)
                    .WithMany(r => r.Users)
                    .HasForeignKey(e => e.RoleId)
                    .OnDelete(DeleteBehavior.Restrict); 
            });
            modelBuilder.Entity<UsersModel>()
                .HasMany(u => u.ShoppingCartItems)
                .WithOne(sci => sci.User)
                .HasForeignKey(sci => sci.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<RolesModel>(entity =>
            {
                entity.HasKey(e => e.RoleId);
                entity.Property(e => e.RoleName).IsRequired();
            });

            // Cấu hình mối quan hệ một-nhiều giữa Category và SubCategory
            modelBuilder.Entity<SubCategory>()
                .HasOne(sc => sc.Category)
                .WithMany(c => c.SubCategories)
                .HasForeignKey(sc => sc.CategoryID)
                .OnDelete(DeleteBehavior.Cascade); // Hoặc Restrict nếu cần

            // Cấu hình mối quan hệ một-nhiều giữa SubCategory và Product
            modelBuilder.Entity<Product>()
                .HasOne(p => p.SubCategory)
                .WithMany(sc => sc.Products)
                .HasForeignKey(p => p.SubCategoryID)
                .OnDelete(DeleteBehavior.Cascade); // Hoặc Restrict nếu cần

            // Cấu hình mối quan hệ một-nhiều giữa Product và FlashSale
            modelBuilder.Entity<Product>()
                .HasMany(p => p.FlashSales)
                .WithOne(fs => fs.Product)
                .HasForeignKey(fs => fs.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // Cấu hình kiểu dữ liệu decimal với độ chính xác và tỷ lệ
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<FlashSale>()
                .Property(f => f.FlashSalePrice)
                .HasColumnType("decimal(18,2)");

            // Định nghĩa khóa chính cho ShoppingCartItem
            modelBuilder.Entity<ShoppingCartItem>()
                .HasKey(c => c.ShoppingCartItemId);
            // Định nghĩa khóa ngoại cho ShoppingCartId
            modelBuilder.Entity<ShoppingCartItem>()
               .HasOne(sci => sci.User)
               .WithMany(u => u.ShoppingCartItems)
               .HasForeignKey(sci => sci.UserId)
               .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<ShoppingCartItem>()
                .HasOne(sci => sci.Product)
                .WithMany(p => p.ShoppingCartItems)
                .HasForeignKey(sci => sci.ProductId)
                .OnDelete(DeleteBehavior.Restrict); // Tránh cascade path

            modelBuilder.Entity<ShoppingCartItem>()
                .HasOne(sci => sci.ShoppingCart)
                .WithMany(sc => sc.ShoppingCartItems)
                .HasForeignKey(sci => sci.ShoppingCartId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ShoppingCart>()
                .HasMany(c => c.ShoppingCartItems)
                .WithOne(i => i.ShoppingCart)
                .HasForeignKey(i => i.ShoppingCartId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ShoppingCartItem>()
                .HasIndex(c => c.ShoppingCartId)
                .IsUnique(false);

            modelBuilder.Entity<ShoppingCartItem>()
                .Property(c => c.Price)
                .HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Users)
                .WithMany(u => u.Orders) 
                .HasForeignKey(o => o.UserId) 
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderDetails)
                .WithOne(od => od.Order)
                .HasForeignKey(od => od.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Order>()
                .Property(c => c.TotalPrice)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<OrderDetail>()
                .Property(c => c.Price)
                .HasColumnType("decimal(18,2)");
            modelBuilder.Entity<ConstructionBooking>()
              .HasOne(cb => cb.Users)
              .WithMany(u => u.ConstructionBookings)
              .HasForeignKey(cb => cb.UserId)
              .OnDelete(DeleteBehavior.Cascade);
            // Seed dữ liệu
            modelBuilder.Entity<RolesModel>().HasData(
               new RolesModel { RoleId = 1, RoleName = "Quản lý" },
               new RolesModel { RoleId = 2, RoleName = "Khách hàng" }
           );
            modelBuilder.Entity<UsersModel>().HasData(
              new UsersModel
              {
                  UserId = 1,
                  UserName = "Nguyễn Anh Tú",
                  UserEmail = "AnhTu080302@gmail.com",
                  Password = HashPassword("AnhTu080302@"),
                  ProfilePicture = "",
                  Address = "Bình Dương",
                  PhoneNumber = "0332613703",
                  RoleId = 1,
              }
          );
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryID = 1, CategoryName = "Ấn Phẩm Văn Phòng" }
              
            );

            modelBuilder.Entity<SubCategory>().HasData(
                new SubCategory { SubCategoryID = 1, CategoryID = 1, SubCategoryName = "Danh Thiếp" },
                new SubCategory { SubCategoryID = 2, CategoryID = 1, SubCategoryName = "Bao Thư" },
                new SubCategory { SubCategoryID = 3, CategoryID = 1, SubCategoryName = "Bìa Đựng Hồ Sơ" }
            );
            modelBuilder.Entity<Product>().HasData(
                new Product { ProductID = 1, SubCategoryID = 1, ProductName = "Danh Thiếp 1", Description = "Mô tả sản phẩm 2", ImageFileName = "DanhThiep_1.jpg",Price = 100000, Size = "80x100cm",CategoryProduct = "Hộp"},
                new Product { ProductID = 2, SubCategoryID = 1, ProductName = "Danh Thiếp 2", Description = "Mô tả sản phẩm 3", ImageFileName = "DanhThiep_1.jpg", Price = 100000, Size = "80x100cm", CategoryProduct = "Hộp"},
                new Product { ProductID = 3, SubCategoryID = 2, ProductName = "Bao Thư 1", Description = "Mô tả sản phẩm 2", ImageFileName = "bao.jpg", Price = 100000, Size = "80x100cm", CategoryProduct = "Hộp" },
                new Product { ProductID = 4, SubCategoryID = 2, ProductName = "Bao Thư 2", Description = "Mô tả sản phẩm 3", ImageFileName = "bao.jpg" , Price = 100000, Size = "80x100cm", CategoryProduct = "Hộp" },
                new Product { ProductID = 5, SubCategoryID = 3, ProductName = "Bìa Đựng Hồ Sơ 1", Description = "Mô tả sản phẩm 2", ImageFileName = "TuiGiay_1.jpg", Price = 100000, Size = "80x100cm", CategoryProduct = "Hộp" },
                new Product { ProductID = 6, SubCategoryID = 3, ProductName = "Bìa Đựng Hồ Sơ 2", Description = "Mô tả sản phẩm 3", ImageFileName = "TuiGiay_1.jpg" , Price = 100000, Size = "80x100cm", CategoryProduct = "Hộp" }
            );
            modelBuilder.Entity<FlashSale>().HasData(
                 new FlashSale { FlashSaleId = 1, ProductId = 1, FlashSalePrice = 800, FlashSaleStartTime = DateTime.Now.AddDays(-1), FlashSaleEndTime = DateTime.Now.AddDays(1), IsActive = true },
                 new FlashSale { FlashSaleId = 2, ProductId = 2, FlashSalePrice = 1500, FlashSaleStartTime = DateTime.Now.AddDays(-1), FlashSaleEndTime = DateTime.Now.AddDays(1), IsActive = true }
             );
        }
      
    }
 
}
