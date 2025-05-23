using NodaTime;

namespace GambitoServer.LinhaProducao;

public partial class LinhaProducaoDiaEntity
{
  public int LinhaProducao { get; set; }

  public LocalDate? Data { get; set; }

  public bool Invativo { get; set; }

  public virtual ICollection<LinhaProducaoHoraEntity> LinhaProducaoHoras { get; set; } = new List<LinhaProducaoHoraEntity>();

  public virtual LinhaProducaoEntity LinhaProducaoNavigation { get; set; } = null!;
}
