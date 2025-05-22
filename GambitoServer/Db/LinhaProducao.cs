using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GambitoServer.Db;

public partial class LinhaProducao
{
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public int Id { get; set; }

  public string? Descricao { get; set; }

  [JsonIgnore]
  public virtual ICollection<LinhaProducaoDia> LinhaProducaoDia { get; set; } = new List<LinhaProducaoDia>();
}

public record LinhaProducaoModel(int Id, string? Descricao, IList<LinhaProducaoDia> Dias);
