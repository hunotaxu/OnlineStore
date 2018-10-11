using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace OnlineStore.Models
{
    public partial class OnlineStoreDBContext : DbContext
    {
        public OnlineStoreDBContext()
        {
        }

        public OnlineStoreDBContext(DbContextOptions<OnlineStoreDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Cart> Cart { get; set; }
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
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=DESKTOP-L5BRUUB\\SQLEXPRESS;Database=OnlineStoreDB1;Trusted_Connection=True;MultipleActiveResultSets=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cart>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Timestamp).IsRowVersion();

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Cart)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_Cart_User");
            });

            modelBuilder.Entity<CartDetail>(entity =>
            {
                entity.HasKey(e => new { e.CartId, e.ItemId });

                entity.Property(e => e.Timestamp).IsRowVersion();

                entity.HasOne(d => d.Cart)
                    .WithMany(p => p.CartDetail)
                    .HasForeignKey(d => d.CartId)
                    .HasConstraintName("FK_CartDetail_Cart");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.CartDetail)
                    .HasForeignKey(d => d.ItemId)
                    .HasConstraintName("FK_CartDetail_Item");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(10);

                entity.Property(e => e.Timestamp).IsRowVersion();
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.Property(e => e.Content).HasMaxLength(200);

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.Comment)
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Comment_Item");
            });

            modelBuilder.Entity<Event>(entity =>
            {
                entity.Property(e => e.Bonus).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.Property(e => e.Timestamp).IsRowVersion();
            });

            modelBuilder.Entity<GoodsReceipt>(entity =>
            {
                entity.Property(e => e.Timestamp).IsRowVersion();

                entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.Supplier)
                    .WithMany(p => p.GoodsReceipt)
                    .HasForeignKey(d => d.SupplierId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_GoodsReceipt_Supplier");
            });

            modelBuilder.Entity<GoodsReceiptDetail>(entity =>
            {
                entity.HasKey(e => new { e.GoodsReceiptId, e.ItemId });

                entity.Property(e => e.Timestamp).IsRowVersion();

                entity.HasOne(d => d.GoodsReceipt)
                    .WithMany(p => p.GoodsReceiptDetail)
                    .HasForeignKey(d => d.GoodsReceiptId)
                    .HasConstraintName("FK_GoodsReceiptDetail_GoodsReceipt");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.GoodsReceiptDetail)
                    .HasForeignKey(d => d.ItemId)
                    .HasConstraintName("FK_GoodsReceiptDetail_Item");
            });

            modelBuilder.Entity<Item>(entity =>
            {
                entity.Property(e => e.AverageEvaluation).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.Price).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Timestamp).IsRowVersion();

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Item)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Item_Category");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.Item)
                    .HasForeignKey(d => d.EventId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Item_Event");
            });

            modelBuilder.Entity<LineItem>(entity =>
            {
                entity.HasKey(e => new { e.OrderId, e.ItemId });

                entity.Property(e => e.Amount).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Timestamp).IsRowVersion();

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.LineItem)
                    .HasForeignKey(d => d.ItemId)
                    .HasConstraintName("FK_LineItem_Item");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.LineItem)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK_LineItem_Order");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.Bonus).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.DeliveryDate).HasColumnType("datetime");

                entity.Property(e => e.ShippingFee).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Timestamp).IsRowVersion();

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Order)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Order_User");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Description).HasMaxLength(200);

                entity.Property(e => e.Name).HasMaxLength(200);
            });

            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.Property(e => e.Address).HasMaxLength(500);

                entity.Property(e => e.Email)
                    .HasMaxLength(320)
                    .IsUnicode(false);

                entity.Property(e => e.Name).HasMaxLength(200);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Timestamp).IsRowVersion();
            });

            modelBuilder.Entity<TypeOfUser>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.Timestamp).IsRowVersion();
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Address).HasMaxLength(500);

                entity.Property(e => e.Email)
                    .HasMaxLength(320)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName).HasMaxLength(100);

                entity.Property(e => e.LastName).HasMaxLength(100);

                entity.Property(e => e.Password).IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Timestamp).IsRowVersion();

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.TypeOfUser)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.TypeOfUserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_User_TypeOfUser");
            });

            modelBuilder.Entity<UserDecentralization>(entity =>
            {
                entity.HasKey(e => new { e.TypeOfUserId, e.RoleId });

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.UserDecentralization)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_UserDecentralization_Role");

                entity.HasOne(d => d.TypeOfUser)
                    .WithMany(p => p.UserDecentralization)
                    .HasForeignKey(d => d.TypeOfUserId)
                    .HasConstraintName("FK_UserDecentralization_TypeOfUser");
            });
        }
    }
}
