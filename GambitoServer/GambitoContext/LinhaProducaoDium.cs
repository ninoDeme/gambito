namespace GambitoServer.GambitoContext;

public partial class LinhaProducaoDium
{
    public int LinhaProducao { get; set; }

    public DateOnly Data { get; set; }

    public bool Invativo { get; set; }

    public virtual ICollection<LinhaProducaoHoraEtapa> LinhaProducaoHoraEtapas { get; set; } = new List<LinhaProducaoHoraEtapa>();

    public virtual ICollection<LinhaProducaoHora> LinhaProducaoHoras { get; set; } = new List<LinhaProducaoHora>();

    public virtual LinhaProducao LinhaProducaoNavigation { get; set; } = null!;
}
