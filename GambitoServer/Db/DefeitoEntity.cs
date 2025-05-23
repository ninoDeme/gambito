using GambitoServer.LinhaProducao;

namespace GambitoServer.Db;

public partial class DefeitoEntity
{
  public int Id { get; set; }

  public string Nome { get; set; } = null!;

  public virtual ICollection<LinhaProducaoHoraDefeitoEntity> LinhaProducaoHoraDefeitos { get; set; } = new List<LinhaProducaoHoraDefeitoEntity>();
}
