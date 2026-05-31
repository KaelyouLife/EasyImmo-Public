using System;
using System.Collections.Generic;

namespace DAL.DB;

public partial class TypeTransaction
{
    public int TypeTransactionId { get; set; }

    public string Libelle { get; set; } = null!;

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
