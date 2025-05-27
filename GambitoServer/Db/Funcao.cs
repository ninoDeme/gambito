using System;
using System.Collections.Generic;

namespace GambitoServer.Db;

public partial class Funcao
{
    public int Id { get; set; }

    public int? Organizacao { get; set; }

    public string Nome { get; set; } = null!;

    public virtual ICollection<Funcionario> Funcionarios { get; set; } = new List<Funcionario>();

    public virtual Organizacao? OrganizacaoNavigation { get; set; }
}
