using System;
using System.Collections.Generic;

namespace DAL.DB;

public partial class DocumentBien
{
    public int DocumentBienId { get; set; }

    public string Description { get; set; } = null!;

    public string Chemin { get; set; } = null!;

    public DateOnly DateCreation { get; set; }

    public int BienId { get; set; }

    public int? TypeDocumentId { get; set; }

    public virtual Bien Bien { get; set; } = null!;

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    public virtual TypeDocument? TypeDocument { get; set; }
}
