using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RoleAuthDemo.Models;

namespace RoleAuthDemo.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Member> Members { get; set; }

        // ✅ New DbSet for Plots
        public DbSet<Plot> Plots { get; set; }

        //New DbSet for Bookings
        public DbSet<Booking> Bookings { get; set; }

    }
}
