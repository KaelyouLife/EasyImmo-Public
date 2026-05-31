using System;
using System.Collections.Generic;

namespace DAL.DB;

public partial class TypeEvenement
{
    public int TypeEvenementId { get; set; }

    public string Libelle { get; set; } = null!;

    public virtual ICollection<Evenement> Evenements { get; set; } = new List<Evenement>();
}
