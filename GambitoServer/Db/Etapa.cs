using System;
using System.Collections.Generic;

namespace GambitoServer.Db;

public partial class Etapa: IHasOrg
{
    public int Id { get; set; }

    public int Organizacao { get; set; }

    public string Nome { get; set; } = null!;

    public virtual ICollection<LinhaProducaoHoraEtapa> LinhaProducaoHoraEtapas { get; set; } = new List<LinhaProducaoHoraEtapa>();

    public virtual Organizacao OrganizacaoNavigation { get; set; } = null!;
}
