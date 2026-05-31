using System;
using System.Collections.Generic;

namespace DAL.DB;

public partial class RelationEvenementPersonne
{
    public int RelationEvenementPersonneId { get; set; }

    public string? Commentaire { get; set; }

    public int PersonneId { get; set; }

    public int EvenementId { get; set; }

    public int RoleEvenementPersonneId { get; set; }

    public virtual Evenement Evenement { get; set; } = null!;

    public virtual Personne Personne { get; set; } = null!;

    public virtual RoleEvenementPersonne RoleEvenementPersonne { get; set; } = null!;
}
