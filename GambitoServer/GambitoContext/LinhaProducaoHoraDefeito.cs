namespace GambitoServer.GambitoContext;

public partial class LinhaProducaoHoraDefeito
{
    public int LinhaProducaoHora { get; set; }

    public bool Retrabalhado { get; set; }

    public int Defeito { get; set; }

    public int QtdPecas { get; set; }

    public virtual Defeito DefeitoNavigation { get; set; } = null!;

    public virtual LinhaProducaoHora LinhaProducaoHoraNavigation { get; set; } = null!;
}
