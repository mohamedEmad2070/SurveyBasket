using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace SurveyBasket.Api.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): 
    IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Poll> Polls { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }


}
