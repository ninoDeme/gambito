using System.ComponentModel.DataAnnotations;
using Hydro;
using Hydro.Validation;

namespace GambitoServer.Pages.Components;

public class Counter : HydroComponent
{
  public int Count { get; set; }


  [ValidateCollection]
  public Pessoas[] Names { get; set; }

  public override void Mount()
  {
    Names = [new Pessoas { Nome = "" }];
  }

  public void Add()
  {
    Count++;
  }

  public void PushName()
  {
    Names = [.. Names, new Pessoas { Nome = "" }];
  }

  public string JoinNames()
  {
    return string.Join(", ", Names.Select(n => n.Nome));
  }

  public void Submit() { }
}

public class Pessoas
{
  public Guid Id { get; set; } = Guid.CreateVersion7();

  [MinLength(4)]
  public required string Nome { get; set; }
}
