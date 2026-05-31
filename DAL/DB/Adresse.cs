using System;
using System.Collections.Generic;

namespace DAL.DB;

public partial class Adresse
{
    public int AdresseId { get; set; }

    public string? Numero { get; set; }

    public string? Boite { get; set; }

    public string? Rue { get; set; }

    public string? Commune { get; set; }

    public string? CodePostal { get; set; }

    public virtual ICollection<Bien> Biens { get; set; } = new List<Bien>();

    public virtual ICollection<Personne> Personnes { get; set; } = new List<Personne>();
}
