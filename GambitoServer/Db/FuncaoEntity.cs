namespace GambitoServer.Db;

public partial class FuncaoEntity
{
  public int Id { get; set; }

  public string Nome { get; set; } = null!;

  public virtual ICollection<FuncionarioEntity> Funcionarios { get; set; } = new List<FuncionarioEntity>();
}
