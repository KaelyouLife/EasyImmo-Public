using System;
using System.Collections.Generic;

namespace DAL.DB;

public partial class RelationBienPersonne
{
    public int RelationBienPersonneId { get; set; }

    public string Role { get; set; } = null!;

    public int BienId { get; set; }

    public int PersonneId { get; set; }

    public virtual Bien Bien { get; set; } = null!;

    public virtual Personne Personne { get; set; } = null!;
}
