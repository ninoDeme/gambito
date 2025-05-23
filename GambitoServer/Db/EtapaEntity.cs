using GambitoServer.LinhaProducao;

namespace GambitoServer.Db;

public partial class EtapaEntity
{
  public int Id { get; set; }

  public string Nome { get; set; } = null!;

  public virtual ICollection<LinhaProducaoHoraEtapaEntity> LinhaProducaoHoraEtapas { get; set; } = new List<LinhaProducaoHoraEtapaEntity>();
}
