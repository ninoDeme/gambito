using System.Text.Json.Serialization;

namespace GambitoServer.Db;

public partial class Funcao
{
  public int Id { get; set; }

  public string Nome { get; set; } = null!;

  [JsonIgnore]
  public virtual ICollection<Funcionario> Funcionarios { get; set; } = new List<Funcionario>();
}
