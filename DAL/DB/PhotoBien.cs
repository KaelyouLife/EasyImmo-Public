namespace DAL.DB;

public partial class PhotoBien
{
    public int PhotoBienId { get; set; }

    public bool? EstPrincipale { get; set; }

    public string Chemin { get; set; } = null!;

    public int BienId { get; set; }

    public virtual Bien Bien { get; set; } = null!;
}
