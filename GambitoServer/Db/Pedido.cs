using System.Text.Json.Serialization;

namespace GambitoServer.Db;

public partial class Pedido
{
  public int Id { get; set; }

  public int Produto { get; set; }

  public int QtdPecas { get; set; }

  [JsonIgnore]
  public virtual ICollection<LinhaProducaoHora> LinhaProducaoHoras { get; set; } = new List<LinhaProducaoHora>();

  [JsonIgnore]
  public virtual Produto ProdutoNavigation { get; set; } = null!;
}
