﻿using System;
using System.Collections.Generic;

namespace GambitoServer.Db;

public partial class Funcionario: IHasOrg
{
    public int Id { get; set; }

    public int Organizacao { get; set; }

    public string Nome { get; set; } = null!;

    public int Funcao { get; set; }

    public int? Encarregado { get; set; }

    public bool Invativo { get; set; }

    public virtual Funcionario? EncarregadoNavigation { get; set; }

    public virtual Funcao FuncaoNavigation { get; set; } = null!;

    public virtual ICollection<Funcionario> InverseEncarregadoNavigation { get; set; } = new List<Funcionario>();

    public virtual Organizacao OrganizacaoNavigation { get; set; } = null!;

    public virtual ICollection<LinhaProducaoHoraEtapa> LinhaProducaoHoraEtapas { get; set; } = new List<LinhaProducaoHoraEtapa>();
}
