using System;
using System.Collections.Generic;

namespace GambitoServer.Db;

public partial class LinhaProducaoHoraEtapa
{
    public int Id { get; set; }

    public int LinhaProducaoHora { get; set; }

    public int? Etapa { get; set; }

    public int Ordem { get; set; }

    public int Segundos { get; set; }

    public virtual Etapa? EtapaNavigation { get; set; }

    public virtual LinhaProducaoHora LinhaProducaoHoraNavigation { get; set; } = null!;

    public virtual ICollection<Funcionario> Funcionarios { get; set; } = new List<Funcionario>();
}
