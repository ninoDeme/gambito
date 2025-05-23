using GambitoServer.LinhaProducao;

namespace GambitoServer.Db;

public partial class FuncionarioEntity
{
  public int Id { get; set; }

  public string Nome { get; set; } = null!;

  public int? Funcao { get; set; }

  public int? Encarregado { get; set; }

  public bool Invativo { get; set; }

  public virtual FuncionarioEntity? EncarregadoNavigation { get; set; }

  public virtual FuncaoEntity? FuncaoNavigation { get; set; }

  public virtual ICollection<FuncionarioEntity> InverseEncarregadoNavigation { get; set; } = new List<FuncionarioEntity>();

  public virtual ICollection<LinhaProducaoHoraEtapaEntity> LinhaProducaoHoraEtapas { get; set; } = new List<LinhaProducaoHoraEtapaEntity>();
}
