using System;
using System.Collections.Generic;

namespace DAL.DB;

public partial class TypeBien
{
    public int TypeBienId { get; set; }

    public string Libelle { get; set; } = null!;

    public virtual ICollection<Bien> Biens { get; set; } = new List<Bien>();
}
