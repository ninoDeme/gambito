using System;
using System.Collections.Generic;

namespace GambitoServer.Db;

public partial class Produto
{
    public int Id { get; set; }

    public int Organizacao { get; set; }

    public string Nome { get; set; } = null!;

    public int TempoPeca { get; set; }

    public virtual Organizacao OrganizacaoNavigation { get; set; } = null!

    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
}
