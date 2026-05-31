using System;
using System.Collections.Generic;

namespace DAL.DB;

public partial class Utilisateur
{
    public int UtilisateurId { get; set; }

    public string NomUtilisateur { get; set; } = null!;

    public string MotDePasse { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
