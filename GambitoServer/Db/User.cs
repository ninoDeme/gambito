using Microsoft.AspNetCore.Identity;

namespace GambitoServer.Db;

public class User : IdentityUser
{
  [PersonalData]
  public Guid Organization { get; set; }
};

