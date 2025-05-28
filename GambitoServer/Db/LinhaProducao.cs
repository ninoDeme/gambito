using System;
using System.Collections.Generic;

namespace GambitoServer.Db;

public partial class LinhaProducao: IHasOrg
{
    public int Id { get; set; }

    public int Organizacao { get; set; }

    public string? Descricao { get; set; }

    public virtual ICollection<LinhaProducaoDia> LinhaProducaoDia { get; set; } = new List<LinhaProducaoDia>();

    public virtual Organizacao OrganizacaoNavigation { get; set; } = null!;
}
