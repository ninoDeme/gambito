using System.Text.Json.Serialization;

namespace GambitoServer.Db;

public partial class Etapa
{
  public int Id { get; set; }

  public string Nome { get; set; } = null!;

  [JsonIgnore]
  public virtual ICollection<LinhaProducaoHoraEtapa> LinhaProducaoHoraEtapas { get; set; } = new List<LinhaProducaoHoraEtapa>();
}
