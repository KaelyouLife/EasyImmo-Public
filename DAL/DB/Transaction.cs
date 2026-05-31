using System;
using System.Collections.Generic;

namespace DAL.DB;

public partial class Transaction
{
    public int TransactionId { get; set; }

    public string Reference { get; set; } = null!;

    public decimal Montant { get; set; }

    public DateOnly? DateTransaction { get; set; }

    public string Description { get; set; } = null!;

    public int TypeTransactionId { get; set; }

    public int? StatutTransactionId { get; set; }

    public int BeneficiaireId { get; set; }

    public int PayeurId { get; set; }

    public int BienId { get; set; }

    public int? DocumentId { get; set; }

    public virtual Personne Beneficiaire { get; set; } = null!;

    public virtual Bien Bien { get; set; } = null!;

    public virtual DocumentBien? Document { get; set; }

    public virtual Personne Payeur { get; set; } = null!;

    public virtual StatutTransaction? StatutTransaction { get; set; }

    public virtual TypeTransaction TypeTransaction { get; set; } = null!;
}
