using Microsoft.AspNetCore.Identity;

namespace GambitoServer.Db;

public class User : IdentityUser
{
  public virtual ICollection<OrgEntity> Organizacoes { get; set; } = [];
};

