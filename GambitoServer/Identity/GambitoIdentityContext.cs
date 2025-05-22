using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GambitoServer.Identity;

public class GambitoIdentityContext : IdentityDbContext<User>
{

  public GambitoIdentityContext() { }

  public GambitoIdentityContext(DbContextOptions<GambitoIdentityContext> options) : base(options) { }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    optionsBuilder
      .UseNpgsql("Host=localhost:15432;Username=postgres;Password=postgres;Database=gambito")
      .UseSnakeCaseNamingConvention();
  }

  protected override void OnModelCreating(ModelBuilder builder)
  {
    base.OnModelCreating(builder);

    builder.HasDefaultSchema("identity");
  }
};
