using e_corp.Areas.DATA_2;
using e_corp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace e_corp.Areas.Identity.Data;

public class e_corpIdentityDbContext : IdentityDbContext<AppUser>
{
    public e_corpIdentityDbContext(DbContextOptions<e_corpIdentityDbContext> options)
        : base(options)
    {
    }
    public DbSet<Session> Session { get; set; }

    public DbSet<Booking> Booking { get; set; }

    public DbSet<CoachProfile> CoachProfile { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}
