using System.Text.Json.Serialization;

namespace GambitoServer.Db;

public partial class LinhaProducao
{
    public int Id { get; set; }

    public string? Descricao { get; set; }

    [JsonIgnore]
    public virtual ICollection<LinhaProducaoDia> LinhaProducaoDia { get; set; } = new List<LinhaProducaoDia>();
}
