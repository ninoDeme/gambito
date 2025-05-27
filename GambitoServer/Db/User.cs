using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace GambitoServer.Db;

public partial class User: IdentityUser<Guid>
{
    public virtual ICollection<Organizacao> Organizacaos { get; set; } = new List<Organizacao>();
}
