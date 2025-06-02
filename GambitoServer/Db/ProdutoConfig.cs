using System;
using System.Collections.Generic;

namespace GambitoServer.Db;

public partial class ProdutoConfig
{
    public int Id { get; set; }

    public int Produto { get; set; }

    // public int QtdPecas { get; set; }
    
    public virtual ICollection<ProdutoConfigEtapa> ProdutoConfigEtapas { get; set; } = new List<ProdutoConfigEtapa>();

    public virtual ICollection<LinhaProducaoHora> LinhaProducaoHoras { get; set; } = new List<LinhaProducaoHora>();

    public virtual Produto ProdutoNavigation { get; set; } = null!;
}
