using System;
using System.Collections.Generic;

namespace DAL.DB;

public partial class Personne
{
    public int PersonneId { get; set; }

    public string Prenom { get; set; } = null!;

    public string Nom { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Telephone { get; set; }

    public string Sexe { get; set; } = null!;

    public DateOnly? DateNaissance { get; set; }

    public string? CompteBancaire { get; set; }

    public int? AdresseId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Adresse? Adresse { get; set; }

    public virtual ICollection<RelationBienPersonne> RelationBienPersonnes { get; set; } = new List<RelationBienPersonne>();

    public virtual ICollection<RelationEvenementPersonne> RelationEvenementPersonnes { get; set; } = new List<RelationEvenementPersonne>();

    public virtual ICollection<Transaction> TransactionBeneficiaires { get; set; } = new List<Transaction>();

    public virtual ICollection<Transaction> TransactionPayeurs { get; set; } = new List<Transaction>();
}
