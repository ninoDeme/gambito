using System;
using System.Collections.Generic;
using NodaTime;

namespace GambitoServer.Db;

public partial class LinhaProducaoHora
{
    public int Id { get; set; }

    public int LinhaProducaoDia { get; set; }

    public LocalDate? Data { get; set; }

    public LocalTime Hora { get; set; }

    public int Pedido { get; set; }

    public int? QtdProduzido { get; set; }

    public bool Paralizacao { get; set; }

    public LocalTime? HoraIni { get; set; }

    public LocalTime? HoraFim { get; set; }

    public virtual LinhaProducaoDia LinhaProducaoDiaNavigation { get; set; } = null!;

    public virtual ICollection<LinhaProducaoHoraDefeito> LinhaProducaoHoraDefeitos { get; set; } = new List<LinhaProducaoHoraDefeito>();

    public virtual ICollection<LinhaProducaoHoraEtapa> LinhaProducaoHoraEtapas { get; set; } = new List<LinhaProducaoHoraEtapa>();

    public virtual Pedido PedidoNavigation { get; set; } = null!;
}
