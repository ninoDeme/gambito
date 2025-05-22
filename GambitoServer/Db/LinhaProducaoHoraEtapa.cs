using System.Text.Json.Serialization;

namespace GambitoServer.Db;

public partial class LinhaProducaoHoraEtapa
{
  public int Id { get; set; }

  public int LinhaProducaoHora { get; set; }

  public int? Etapa { get; set; }

  public int Ordem { get; set; }

  public int Segundos { get; set; }

  [JsonIgnore]
  public virtual Etapa? EtapaNavigation { get; set; }

  [JsonIgnore]
  public virtual LinhaProducaoHora? LinhaProducaoHoraNavigation { get; set; }

  [JsonIgnore]
  public virtual ICollection<Funcionario> Funcionarios { get; set; } = new List<Funcionario>();
}
