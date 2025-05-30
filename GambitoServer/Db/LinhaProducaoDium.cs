﻿using System;
using System.Collections.Generic;
using NodaTime;

namespace GambitoServer.Db;

public partial class LinhaProducaoDia
{
    public int Id { get; set; }

    public int LinhaProducao { get; set; }

    public LocalDate? Data { get; set; }

    public bool Invativo { get; set; }

    public virtual ICollection<LinhaProducaoHora> LinhaProducaoHoras { get; set; } = new List<LinhaProducaoHora>();

    public virtual LinhaProducao LinhaProducaoNavigation { get; set; } = null!;
}
