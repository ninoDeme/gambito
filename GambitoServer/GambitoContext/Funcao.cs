namespace GambitoServer.GambitoContext;

public partial class Funcao
{
    public int Id { get; set; }

    public string Nome { get; set; } = null!;

    public virtual ICollection<Funcionario> Funcionarios { get; set; } = new List<Funcionario>();
}
