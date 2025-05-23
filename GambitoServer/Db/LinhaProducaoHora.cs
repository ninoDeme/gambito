using NodaTime;

namespace GambitoServer.Db;

public partial class LinhaProducaoHora
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

  public virtual LinhaProducaoDia? LinhaProducaoDiaNavigation { get; set; }

  public virtual ICollection<LinhaProducaoHoraDefeito> LinhaProducaoHoraDefeitos { get; set; } = new List<LinhaProducaoHoraDefeito>();

  public virtual ICollection<LinhaProducaoHoraEtapa> LinhaProducaoHoraEtapas { get; set; } = new List<LinhaProducaoHoraEtapa>();

  public virtual Pedido PedidoNavigation { get; set; } = null!;
}
