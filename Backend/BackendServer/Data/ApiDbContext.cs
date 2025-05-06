using BackendServer.Models.FixModel;
using BackendServer.Models.ProblemModels;
using BackendServer.Models.UserModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BackendServer.Data;

public class ApiDbContext(DbContextOptions<ApiDbContext> options) 
    : IdentityDbContext<IdentityUser, IdentityRole, string>(options)
{
    public DbSet<Problem> Problems { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Fix> Fixes { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>(entity =>
        {
            var guidListConverter = new ValueConverter<List<Guid>, string>(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(Guid.Parse).ToList()
            );

            var guidListComparer = new ValueComparer<List<Guid>>(
                (c1, c2) => c1.SequenceEqual(c2),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToList()
            );

            entity.Property(u => u.Upvotes)
                .HasConversion(guidListConverter)
                .Metadata.SetValueComparer(guidListComparer);

            entity.Property(u => u.Downvotes)
                .HasConversion(guidListConverter)
                .Metadata.SetValueComparer(guidListComparer);
        });
    }
}