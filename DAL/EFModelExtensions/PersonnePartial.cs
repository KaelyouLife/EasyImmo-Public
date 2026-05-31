namespace DAL.DB;

public partial class Personne
{
    public string NomComplet => $"{Prenom} {Nom}";
}