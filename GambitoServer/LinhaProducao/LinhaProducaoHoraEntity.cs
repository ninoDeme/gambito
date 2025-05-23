using NodaTime;
using GambitoServer.Db;

namespace GambitoServer.LinhaProducao;

public partial class LinhaProducaoHoraEntity
{
  public int Id { get; set; }

  public int LinhaProducao { get; set; }

  public LocalDate? Data { get; set; }

  public LocalTime Hora { get; set; }

  public int Pedido { get; set; }

  public int? QtdProduzido { get; set; }

  public bool Paralizacao { get; set; }

  public LocalTime? HoraIni { get; set; }

  public LocalTime? HoraFim { get; set; }

  public TipoHora? Tipo { get; set; }

  public virtual LinhaProducaoDiaEntity LinhaProducaoDiaNavigation { get; set; } = null!;

  public virtual ICollection<LinhaProducaoHoraDefeitoEntity> LinhaProducaoHoraDefeitos { get; set; } = new List<LinhaProducaoHoraDefeitoEntity>();

  public virtual ICollection<LinhaProducaoHoraEtapaEntity> LinhaProducaoHoraEtapas { get; set; } = new List<LinhaProducaoHoraEtapaEntity>();

  public virtual PedidoEntity PedidoNavigation { get; set; } = null!;
}
