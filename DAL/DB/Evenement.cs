using System;
using System.Collections.Generic;

namespace DAL.DB;

public partial class Evenement
{
    public int EvenementId { get; set; }

    public string? Description { get; set; }

    public DateOnly DateEvenement { get; set; }

    public TimeOnly HeureDebut { get; set; }

    public bool? EstAccompli { get; set; }

    public int TypeEvenementId { get; set; }

    public int? BienId { get; set; }

    public virtual Bien? Bien { get; set; }

    public virtual ICollection<RelationEvenementPersonne> RelationEvenementPersonnes { get; set; } = new List<RelationEvenementPersonne>();

    public virtual TypeEvenement TypeEvenement { get; set; } = null!;
}
