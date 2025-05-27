using System;
using System.Collections.Generic;

namespace GambitoServer.Db;

public partial class LinhaProducao
{
    public int Id { get; set; }

    public int? Organizacao { get; set; }

    public string? Descricao { get; set; }

    public virtual ICollection<LinhaProducaoDium> LinhaProducaoDia { get; set; } = new List<LinhaProducaoDium>();

    public virtual Organizacao? OrganizacaoNavigation { get; set; }
}
