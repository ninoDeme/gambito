using System;
using System.Collections.Generic;

namespace GambitoServer.Db;

public partial class Defeito: IHasOrg
{
    public int Id { get; set; }

    public int Organizacao { get; set; }

    public string Nome { get; set; } = null!;

    public virtual ICollection<LinhaProducaoHoraDefeito> LinhaProducaoHoraDefeitos { get; set; } = new List<LinhaProducaoHoraDefeito>();

    public virtual Organizacao? OrganizacaoNavigation { get; set; }
}
