namespace GambitoServer.GambitoContext;

public partial class Pedido
{
    public int Id { get; set; }

    public int Produto { get; set; }

    public int QtdPecas { get; set; }

    public virtual ICollection<LinhaProducaoHora> LinhaProducaoHoras { get; set; } = new List<LinhaProducaoHora>();

    public virtual Produto ProdutoNavigation { get; set; } = null!;
}
