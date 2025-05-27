using System;
using System.Collections.Generic;

namespace GambitoServer.Db;

public partial class Organizacao
{
    public int Id { get; set; }

    public string Nome { get; set; } = null!;

    public virtual ICollection<Defeito> Defeitos { get; set; } = new List<Defeito>();

    public virtual ICollection<Etapa> Etapas { get; set; } = new List<Etapa>();

    public virtual ICollection<Funcao> Funcaos { get; set; } = new List<Funcao>();

    public virtual ICollection<Funcionario> Funcionarios { get; set; } = new List<Funcionario>();

    public virtual ICollection<LinhaProducao> LinhaProducaos { get; set; } = new List<LinhaProducao>();

    public virtual ICollection<Produto> Produtos { get; set; } = new List<Produto>();

    public virtual ICollection<User> Usuarios { get; set; } = new List<User>();
}
