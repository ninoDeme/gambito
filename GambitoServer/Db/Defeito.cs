using System.Text.Json.Serialization;

namespace GambitoServer.Db;

public partial class Defeito
{
  public int Id { get; set; }

  public string Nome { get; set; } = null!;

  [JsonIgnore]
  public virtual ICollection<LinhaProducaoHoraDefeito> LinhaProducaoHoraDefeitos { get; set; } = new List<LinhaProducaoHoraDefeito>();
}
