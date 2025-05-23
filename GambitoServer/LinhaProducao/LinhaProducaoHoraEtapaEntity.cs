using GambitoServer.Db;

namespace GambitoServer.LinhaProducao;

public partial class LinhaProducaoHoraEtapaEntity
{
  public int Id { get; set; }

  public int LinhaProducaoHora { get; set; }

  public int? Etapa { get; set; }

  public int Ordem { get; set; }

  public int Segundos { get; set; }

  public virtual EtapaEntity? EtapaNavigation { get; set; }

  public virtual LinhaProducaoHoraEntity? LinhaProducaoHoraNavigation { get; set; }

  public virtual ICollection<FuncionarioEntity> Funcionarios { get; set; } = new List<FuncionarioEntity>();
}
