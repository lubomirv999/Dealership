namespace Dealership.Data
{
    using Dealership.Models;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class DealershipDbContext : IdentityDbContext<ApplicationUser>
    {
        public DealershipDbContext(DbContextOptions<DealershipDbContext> options)
            : base(options)
        {
        }

        public DbSet<Car> Cars { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Car>()
                .HasMany(c => c.Images)
                .WithOne(i => i.Car)
                .HasForeignKey(i => i.CarId);

            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
