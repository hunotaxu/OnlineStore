using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.EF
{
    public class OnlineStoreDbContext : DbContext
    {
        public OnlineStoreDbContext()
        {
        }

        public OnlineStoreDbContext(DbContextOptions<OnlineStoreDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Address> Address { get; set; }
        public virtual DbSet<Cart> Cart { get; set; }
		public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<CartDetail> CartDetail { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Comment> Comment { get; set; }
        public virtual DbSet<Event> Event { get; set; }
        public virtual DbSet<GoodsReceipt> GoodsReceipt { get; set; }
        public virtual DbSet<GoodsReceiptDetail> GoodsReceiptDetail { get; set; }
        public virtual DbSet<Item> Item { get; set; }
        public virtual DbSet<LineItem> LineItem { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Supplier> Supplier { get; set; }
        public virtual DbSet<TypeOfUser> TypeOfUser { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserDecentralization> UserDecentralization { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#pragma warning disable 1030
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
#pragma warning restore 1030
                optionsBuilder.UseSqlServer("Server=DESKTOP-L5BRUUB\\SQLEXPRESS;Database=OnlineStoreDB;User Id=sa;Password=15110376;Trusted_Connection=True;MultipleActiveResultSets=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CartDetail>().HasKey(c => new { c.ItemId, c.CartId });
            modelBuilder.Entity<GoodsReceiptDetail>().HasKey(c => new { c.ItemId, c.GoodsReceiptId });
            modelBuilder.Entity<LineItem>().HasKey(c => new { c.ItemId, c.OrderId });
            modelBuilder.Entity<UserDecentralization>().HasKey(c => new { c.RoleId, c.TypeOfUserId });

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Điện thoại" },
                new Category { Id = 2, Name = "Laptop" },
                new Category { Id = 3, Name = "Máy tính bảng" },
                new Category { Id = 4, Name = "Phụ kiện" }
            );


            modelBuilder.Entity<Item>().HasData(
                new Item { Id = 1, Name = "Iphone 5", CategoryId = 1, Deleted = false, BrandName="Apple", Description = "Đây là Iphone 5", Price = 5000000, Image = "Iphone5-16G.png", View = 0, Inventory = 200 },
                new Item { Id = 3, Name = "Iphone 5", CategoryId = 1, Deleted = false, BrandName = "Apple", Description = "<div id=\"CauHinh\"><div class=\"CauHinh_TieuDe\">Bộ vi xử lý</div><ul><li>+ Tên bộ vi xử lý : Chip A7 Dual-Core.</li><li>+ Tốc độ : 1.3Ghz Cyclone.</li> <li>+ Bộ nhớ đệm : ARM v8-based</li><li>+ Hệ điều hành : iOS 7.</li> </ul> <div style=\"clear:both\"></div> <div class=\"CauHinh_TieuDe\">Bộ nhớ trong</div> <ul> <li>+ RAM : 1 GB.</li> <li>+ ROM : 16 GB.</li> </ul><div style=\"clear:both\"></div><div class=\"CauHinh_TieuDe\">Màn hình (Display)</div> <ul> <li>+ Độ lớn màn hình : 7.9 inches.</li> <li>+ Độ phân giải : LED-backlit IPS LCD, 16 triệu màu.</li> </ul><div style=\"clear:both\"></div><div class=\"CauHinh_TieuDe\">Camera</div><ul><li>+ Chính : 5 MP , 2592x1944 pixels.</li><li>+ Phụ : 1.2 MP , 720p  </li> <li>+ Chức năng : Nhận diện khuôn mặt, FaceTime, tự động lấy nét.</li><li>+ Quay phim : Full HD 1080p 30fps, chống rung.</li> </ul> <div style=\"clear:both\"></div> <div class=\"CauHinh_TieuDe\">Thông tin thêm</div> <ul><li>+ Kết nối : Wifi, GPRS, 3G, Bluetooth, USB, GPS.</li><li>+ Pin : Li-Po (23.8 Wh), Lên đến 10 giờ.</li><li>+ Trọng lượng : 341g.</li><li>+ Thời gian bảo hành : 24 tháng.</li>  </ul> <div style=\"clear:both\"></div> </div>", Price = 5000000, Image = "Iphone5-16G.png", View = 0, Inventory = 200 },
                new Item { Id = 4, Name = "DELL 3440", CategoryId = 2, Deleted = false, BrandName = "Dell", Description = "Đây là DELL", Price = 12390000, Image = "AP-ME294ZP.png", View = 0, Inventory = 300 },
                new Item { Id = 5, Name = "ACER-2215i", CategoryId = 2, Deleted = false, BrandName = "Acer", Description = "<div id=\"CauHinh\" > <div class=\"CauHinh_TieuDe\" >Bộ vi xử lý</div> <ul> <li>+ Tên bộ vi xử lý : Intel® Core™ i7 380M.</li> <li>+ Tốc độ : 8.0Ghz.</li><li>+ Bộ nhớ đệm : 3MB Cache L3</li> </ul> <div style=\"clear:both\"></div><div class=\"CauHinh_TieuDe\">Bộ nhớ trong (RAM)</div><ul> <li>+ Loại Ram : DDR3 1066Mhz (PC3-8500).</li> <li>+ Dung lượng : 2GB.</li></ul> <div style=\"clear:both\"></div><div class=\"CauHinh_TieuDe\">Ổ đĩa cứng HDD</div> <ul> <li>+ Dung lượng : 320GB.</li><li>+ Kích thước : 2.5 inchs.</li> <li>+ Tốc độ vòng quay : 5400 rpm.</li></ul> <div style=\"clear:both\"></div> <div class=\"CauHinh_TieuDe\">Ổ đĩa quang (ODD)</div> <ul><li>+ Loại ổ đĩa quang : DVD+/- R/RW.</li>  </ul><div style=\"clear:both\"></div><div class=\"CauHinh_TieuDe\">Màn hình (Display)</div><ul><li>+ Độ lớn màn hình : 16 inchs.</li><li>+ Độ phân giải : HD (1366 x 768).</li> </ul> <div style=\"clear:both\"></div> <div class=\"CauHinh_TieuDe\">Đồ Họa (VGA)</div><ul><li>+ Bộ xử lý : Intel HD graphics.</li><li>+ Bộ nhớ đồ họa : Upto 1696MB.</li></ul> <div style=\"clear:both\"></div><div class=\"CauHinh_TieuDe\">Thông tin thêm</div><ul><li>+ Trọng lượng : 3.5 Kg.</li><li>+ Thời gian bảo hành : 24 Tháng.</li></ul><div style=\"clear:both\"></div></div>", Price = 5000000, Image = "ACER-2215i.png", View = 0, Inventory = 200 },
                new Item { Id = 6, Name = "Galaxy Tab-3-10-P5200", CategoryId = 3, Deleted = false, BrandName = "Samsung", Description = "Đây là Galaxy Tab 3", Price = 5904568, Image = "AP-ME294ZP.png", View = 0, Inventory = 200 },
                new Item { Id = 8, Name = "Tai nghe", CategoryId = 4, Deleted = false, Description = "Đây là tai nghe", Price = 5000000, Image = "Iphone5-16G.png", View = 0, Inventory = 200 }            
            );
        }
    }
}
