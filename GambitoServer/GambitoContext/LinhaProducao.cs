namespace GambitoServer.GambitoContext;

public partial class LinhaProducao
{
    public int Id { get; set; }

    public string? Descricao { get; set; }

    public virtual ICollection<LinhaProducaoDium> LinhaProducaoDia { get; set; } = new List<LinhaProducaoDium>();
}
