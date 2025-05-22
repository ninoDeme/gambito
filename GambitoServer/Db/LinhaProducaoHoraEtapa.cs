namespace GambitoServer.Db;

public partial class LinhaProducaoHoraEtapa
{
    public int Id { get; set; }

    public int LinhaProducao { get; set; }

    public DateOnly? Data { get; set; }

    public int? Etapa { get; set; }

    public int Ordem { get; set; }

    public int Segundos { get; set; }

    public virtual Etapa? EtapaNavigation { get; set; }

    public virtual LinhaProducaoDia? LinhaProducaoDia { get; set; }

    public virtual ICollection<Funcionario> Funcionarios { get; set; } = new List<Funcionario>();
}
