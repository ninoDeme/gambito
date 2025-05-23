﻿using GambitoServer.Db;

namespace GambitoServer.LinhaProducao;

public partial class LinhaProducaoEntity
{
  public int Id { get; set; }

  public string? Descricao { get; set; }

  public Guid Org { get; set; }

  public virtual OrgEntity OrgNavigation { get; set; } = null!;

  public static LinhaProducaoEntity FromModel(LinhaProducaoModel m)
  {
    return new LinhaProducaoEntity
    {
      Descricao = m.Descricao,
      Id = m.Id
    };
  }

  public virtual ICollection<LinhaProducaoDiaEntity> LinhaProducaoDia { get; set; } = [];
}

public record LinhaProducaoModel(int Id, string? Descricao)
{
  public LinhaProducaoModel(LinhaProducaoEntity l) : this(l.Id, l.Descricao)
  {
  }
}

