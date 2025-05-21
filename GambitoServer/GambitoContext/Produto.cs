namespace GambitoServer.GambitoContext;

public partial class Produto
{
    public int Id { get; set; }

    public string Nome { get; set; } = null!;

    public int TempoPeca { get; set; }

    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
}
