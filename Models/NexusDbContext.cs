using Microsoft.EntityFrameworkCore;

namespace Nexus_Manegement.Models
{
    public class NexusDbContext :DbContext
    {
        public NexusDbContext(DbContextOptions<NexusDbContext> options)
            : base(options)
        {
        }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Admin> Admins { get; set; }

        //
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>(e =>
            {
                e.ToTable("Admin");
            });
            //base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Position>(e =>
            {
                e.ToTable("Position");
                e.HasKey(pk => pk.ID);
                e.Property(n => n.Name)
                .HasColumnType("varchar")
                .HasMaxLength(100);
            });
            modelBuilder.Entity<Employee>(e =>
            {
                e.ToTable("Employee");
                e.HasKey(pk => pk.ID);
                e.Property(n => n.Name)
                .HasColumnType("varchar")
                .HasMaxLength(30);
                e.Property(u => u.Username)
                .HasColumnType("varchar")
                .HasMaxLength(50);
                e.Property(p => p.Password)
                .HasColumnType("varchar")
                .HasMaxLength(200);
                e.Property(p => p.Phone)
                .HasColumnType("varchar")
                .HasMaxLength(15);
                e.Property(e => e.Email)
                .HasColumnType("varchar")
                .HasMaxLength(60);
                e.Property(s => s.Status)
                .HasColumnType("bit");
                e.Property(fk => fk.Position)
                .HasColumnType("int");


            });

        }
    }
}
