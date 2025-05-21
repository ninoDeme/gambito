namespace GambitoServer.GambitoContext;

public partial class LinhaProducaoHora
{
    public int Id { get; set; }

    public int LinhaProducao { get; set; }

    public DateOnly? Data { get; set; }

    public TimeOnly Hora { get; set; }

    public int Pedido { get; set; }

    public int? QtdProduzido { get; set; }

    public bool Paralizacao { get; set; }

    public TimeOnly? HoraIni { get; set; }

    public TimeOnly? HoraFim { get; set; }

    public TipoHora? Tipo { get; set; }

    public virtual LinhaProducaoDium? LinhaProducaoDium { get; set; }

    public virtual ICollection<LinhaProducaoHoraDefeito> LinhaProducaoHoraDefeitos { get; set; } = new List<LinhaProducaoHoraDefeito>();

    public virtual Pedido PedidoNavigation { get; set; } = null!;
}
