using System;
using System.Collections.Generic;

namespace DAL.DB;

public partial class TypeDocument
{
    public int TypeDocumentId { get; set; }

    public string? Libelle { get; set; }

    public virtual ICollection<DocumentBien> DocumentBiens { get; set; } = new List<DocumentBien>();
}
