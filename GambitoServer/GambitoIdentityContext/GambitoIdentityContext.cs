using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GambitoServer.GambitoIdentityContext;

public class GambitoIdentityContext : IdentityDbContext<User>
{
  public GambitoIdentityContext(DbContextOptions<GambitoIdentityContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema("identity");
    }
};
