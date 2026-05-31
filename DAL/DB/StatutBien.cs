using System;
using System.Collections.Generic;

namespace DAL.DB;

public partial class StatutBien
{
    public int StatutBienId { get; set; }

    public string Libelle { get; set; } = null!;

    public virtual ICollection<Bien> Biens { get; set; } = new List<Bien>();

    public virtual ICollection<HistoriqueStatutBien> HistoriqueStatutBiens { get; set; } = new List<HistoriqueStatutBien>();
}
