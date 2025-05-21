namespace GambitoServer.GambitoContext;

public partial class Etapa
{
    public int Id { get; set; }

    public string Nome { get; set; } = null!;

    public virtual ICollection<LinhaProducaoHoraEtapa> LinhaProducaoHoraEtapas { get; set; } = new List<LinhaProducaoHoraEtapa>();
}
