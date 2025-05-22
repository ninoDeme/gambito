using System.Text.Json.Serialization;
using NodaTime;

namespace GambitoServer.Db;

public partial class LinhaProducaoDia
{
  public int LinhaProducao { get; set; }

  public LocalDate Data { get; set; }

  public bool Invativo { get; set; }

  [JsonIgnore]
  public virtual ICollection<LinhaProducaoHora> LinhaProducaoHoras { get; set; } = new List<LinhaProducaoHora>();

  [JsonIgnore]
  public virtual LinhaProducao LinhaProducaoNavigation { get; set; } = null!;
}
