namespace GambitoServer.Db;

public partial class LinhaProducao
{
  public int Id { get; set; }

  public string? Descricao { get; set; }

  public static LinhaProducao FromModel(LinhaProducaoModel m)
  {
    return new LinhaProducao
    {
      Descricao = m.Descricao,
      Id = m.Id
    };
  }

  public virtual ICollection<LinhaProducaoDia> LinhaProducaoDia { get; set; } = [];
}

public record LinhaProducaoModel(int Id, string? Descricao)
{
  public LinhaProducaoModel(LinhaProducao l) : this(l.Id, l.Descricao)
  {
  }
}

