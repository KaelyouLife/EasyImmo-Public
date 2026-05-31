using System;
using System.Collections.Generic;

namespace DAL.DB;

public partial class HistoriqueStatutBien
{
    public int HistoriqueStatutBienId { get; set; }

    public DateTime? DateChangement { get; set; }

    public int StatutBienId { get; set; }

    public int BienId { get; set; }

    public virtual Bien Bien { get; set; } = null!;

    public virtual StatutBien StatutBien { get; set; } = null!;
}
