using System;
using System.Collections.Generic;

namespace DAL.DB;

public partial class StatutTransaction
{
    public int StatutTransactionId { get; set; }

    public string? Libelle { get; set; }

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
