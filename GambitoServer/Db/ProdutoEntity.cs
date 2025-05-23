namespace GambitoServer.Db;

public partial class ProdutoEntity
{
  public int Id { get; set; }

  public string Nome { get; set; } = null!;

  public int TempoPeca { get; set; }

  public virtual ICollection<PedidoEntity> Pedidos { get; set; } = new List<PedidoEntity>();
}
