using GambitoServer.LinhaProducao;

namespace GambitoServer.Db;

public partial class PedidoEntity
{
  public int Id { get; set; }

  public int Produto { get; set; }

  public int QtdPecas { get; set; }

  public virtual ICollection<LinhaProducaoHoraEntity> LinhaProducaoHoras { get; set; } = new List<LinhaProducaoHoraEntity>();

  public virtual ProdutoEntity ProdutoNavigation { get; set; } = null!;
}
