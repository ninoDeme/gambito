using System.Text.Json.Serialization;

namespace GambitoServer.Db;

public partial class LinhaProducaoHoraDefeito
{
  public int LinhaProducaoHora { get; set; }

  public bool Retrabalhado { get; set; }

  public int Defeito { get; set; }

  public int QtdPecas { get; set; }

  [JsonIgnore]
  public virtual Defeito DefeitoNavigation { get; set; } = null!;

  [JsonIgnore]
  public virtual LinhaProducaoHora LinhaProducaoHoraNavigation { get; set; } = null!;
}
