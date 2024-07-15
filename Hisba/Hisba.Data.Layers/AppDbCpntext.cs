using Hisba.Data.Layers.Entities;
using Hisba.Data.Layers.Migrations;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hisba.Data.Layers
{
    public class AppDbContext : DbContext
    {
        public static readonly string ConnectionString = "Data Source=.; Initial Catalog=InventoryMsDB; MultipleActiveResultSets=True;;Integrated Security=True";


        public AppDbContext() : this(ConnectionString) { }

        public AppDbContext(string ConnectionString) : base(ConnectionString)
        {
            Database.CreateIfNotExists();
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<AppDbContext, Configuration>());

            Database.Initialize(false);
        }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<OrderType> OrderTypes { get; set; }

        public DbSet<Tier> Tiers { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductCategory> ProductCategories { get; set; }

        public DbSet<Deposit> Deposits { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<TransactionType> TransactionTypes { get; set; }

        public DbSet<User> Users { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            // One-To-Many: Tier - Order
            modelBuilder.Entity<Order>().HasRequired(o => o.Tier).WithMany(o => o.Orders)
                                        .HasForeignKey(o => o.TierId).WillCascadeOnDelete(false);

            // One-To-Many: OrderType - Order
            modelBuilder.Entity<Order>().HasRequired(o => o.OrderType).WithMany(o => o.Orders)
                                        .HasForeignKey(o => o.OrderTypeId).WillCascadeOnDelete(false);

            // One-To-Many: Order - OrderItem
            modelBuilder.Entity<OrderItem>().HasRequired(oi => oi.Order).WithMany(o => o.Items)
                                            .HasForeignKey(o => o.OrderId).WillCascadeOnDelete(false);

            // One-To-Many: Product - OrderItem
            modelBuilder.Entity<OrderItem>().HasRequired(oi => oi.Product).WithMany(o => o.Items)
                                            .HasForeignKey(o => o.ProductId).WillCascadeOnDelete(false);

            // One-To-Many: ProductCategory - Product
            modelBuilder.Entity<Product>().HasRequired(p => p.Category).WithMany(c => c.Products)
                                          .HasForeignKey(p => p.CategoryId).WillCascadeOnDelete(false);

            // One-To-Many: Order - Deposit
            modelBuilder.Entity<Deposit>().HasRequired(d => d.Order).WithMany(o => o.Deposits)
                                          .HasForeignKey(o => o.OrderId).WillCascadeOnDelete(false);

            // One-To-Many: Transaction - Deposit
            modelBuilder.Entity<Deposit>().HasRequired(d => d.Transaction).WithMany(o => o.Deposits)
                                          .HasForeignKey(o => o.TransactionId).WillCascadeOnDelete(false);

            // One-To-Many: Tier - Transaction
            modelBuilder.Entity<Transaction>().HasRequired(d => d.Tier).WithMany(o => o.Transactions)
                                          .HasForeignKey(o => o.TierId).WillCascadeOnDelete(false);

            // One-To-Many: TransactionType - Transaction
            modelBuilder.Entity<Transaction>().HasRequired(d => d.Type).WithMany(o => o.Transactions)
                                          .HasForeignKey(o => o.TypeId).WillCascadeOnDelete(false);

            modelBuilder.Entity<User>().HasOptional(u => u.Creator)
                                       .WithMany(u => u.CreatedUsers).HasForeignKey(u => u.CreatorId);


            modelBuilder.Conventions.Remove<PluralizingEntitySetNameConvention>();

            base.OnModelCreating(modelBuilder);
        }
    }
}
