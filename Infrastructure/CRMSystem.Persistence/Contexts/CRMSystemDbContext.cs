using CRMSystem.Domain.Entities;
using CRMSystem.Persistence.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CRMSystem.Persistence.Contexts
{
    public class CRMSystemDbContext : IdentityDbContext<Admin>
    {
        public CRMSystemDbContext(DbContextOptions<CRMSystemDbContext> options) : base(options)
        {
        }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<OrderOverhead> OrderOverheads { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<MonthlyCalculation> MonthlyCalculation { get; set; }

        public DbSet<PendingCategory> PendingCategories { get; set; }
        public DbSet<PendingProduct> PendingProducts { get; set; }
        public DbSet<AdminNotification> AdminNotifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new AdminConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new CompanyConfiguration());
            modelBuilder.ApplyConfiguration(new DepartmentConfiguration());
            modelBuilder.ApplyConfiguration(new JobConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new OrderItemConfiguration());
            modelBuilder.ApplyConfiguration(new OrderOverheadConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new SectionConfiguration());
            modelBuilder.ApplyConfiguration(new VendorConfiguration());
            modelBuilder.ApplyConfiguration(new PendingCategoryConfiguration());
            modelBuilder.ApplyConfiguration(new PendingProductConfiguration());
            modelBuilder.ApplyConfiguration(new AdminNotificationConfiguration());
            modelBuilder.ApplyConfiguration(new MonthlyCalculationConfiguration());
        }
    }
}
