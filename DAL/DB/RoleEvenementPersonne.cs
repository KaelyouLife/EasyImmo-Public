using System;
using System.Collections.Generic;

namespace DAL.DB;

public partial class RoleEvenementPersonne
{
    public int RoleEvenementPersonneId { get; set; }

    public string Libelle { get; set; } = null!;

    public virtual ICollection<RelationEvenementPersonne> RelationEvenementPersonnes { get; set; } = new List<RelationEvenementPersonne>();
}
