using Microsoft.EntityFrameworkCore;
using Nexus_Manegement.Areas.Management.ModelView;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nexus_Manegement.Models
{
    public class NexusContext:DbContext
    {
        public NexusContext()
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Data Source=viet;Initial Catalog=Nexus;Persist Security Info=True;User ID=sa;Password=123;TrustServerCertificate=True");
        }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<ProductExchange> ProductExchanges { get; set; }
        public DbSet<ServicePlan> ServicePlans { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<ExpiryDate> ExpiryDate { get; set; }
        public DbSet<Option> Options { get; set; }
 
      
    }
}
