using GambitoServer.Db;

namespace GambitoServer.LinhaProducao;

public partial class LinhaProducaoHoraDefeitoEntity
{
  public int LinhaProducaoHora { get; set; }

  public bool Retrabalhado { get; set; }

  public int Defeito { get; set; }

  public int QtdPecas { get; set; }

  public virtual DefeitoEntity DefeitoNavigation { get; set; } = null!;

  public virtual LinhaProducaoHoraEntity LinhaProducaoHoraNavigation { get; set; } = null!;
}
