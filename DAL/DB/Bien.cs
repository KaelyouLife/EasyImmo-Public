using System;
using System.Collections.Generic;

namespace DAL.DB;

public partial class Bien
{
    public int BienId { get; set; }

    public string Reference { get; set; } = null!;

    public string TitreAnnonce { get; set; } = null!;

    public string Peb { get; set; } = null!;

    public decimal? PrixVente { get; set; }

    public decimal? LoyerMensuel { get; set; }

    public decimal? ChargesMensuelles { get; set; }

    public bool? EstLibre { get; set; }

    public string? Description { get; set; }

    public decimal? SurfaceHabitable { get; set; }

    public decimal? SurfaceTotale { get; set; }

    public int? NombrePieces { get; set; }

    public int? NombreChambres { get; set; }

    public int? NombreSalleBain { get; set; }

    public int? NombreWc { get; set; }

    public string? AnneeConstruction { get; set; }

    public bool? Ascenseur { get; set; }

    public string? Chauffage { get; set; }

    public string? Isolation { get; set; }

    public bool? CuisineEquipee { get; set; }

    public bool? Cave { get; set; }

    public bool? Grenier { get; set; }

    public bool? Garage { get; set; }

    public bool? Jardin { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int AdresseId { get; set; }

    public int TypeBienId { get; set; }

    public int StatutBienId { get; set; }

    public virtual Adresse Adresse { get; set; } = null!;

    public virtual ICollection<DocumentBien> DocumentBiens { get; set; } = new List<DocumentBien>();

    public virtual ICollection<Evenement> Evenements { get; set; } = new List<Evenement>();

    public virtual ICollection<HistoriqueStatutBien> HistoriqueStatutBiens { get; set; } = new List<HistoriqueStatutBien>();

    public virtual ICollection<PhotoBien> PhotoBiens { get; set; } = new List<PhotoBien>();

    public virtual ICollection<RelationBienPersonne> RelationBienPersonnes { get; set; } = new List<RelationBienPersonne>();

    public virtual StatutBien StatutBien { get; set; } = null!;

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    public virtual TypeBien TypeBien { get; set; } = null!;
}
