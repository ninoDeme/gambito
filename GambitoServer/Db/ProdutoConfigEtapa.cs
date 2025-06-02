using System;
using System.Collections.Generic;

namespace GambitoServer.Db;

public partial class ProdutoConfigEtapa
{
    public int ProdutoConfig { get; set; }

    public int Etapa { get; set; }

    public int Segundos { get; set; }

    public int Ordem { get; set; }

    public virtual Etapa EtapaNavigation { get; set; } = null!;

    public virtual ProdutoConfig ProdutoConfigNavigation { get; set; } = null!;
}
