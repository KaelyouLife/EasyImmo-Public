using BU.Services;
using Common.Utilities;
using DAL.DB;

namespace EasyImmoMAUI.ViewModels.BiensViewModels;

public class AjouterBienViewModel
{
    public string Reference { get; set; } = string.Empty;
    public string TitreAnnonce { get; set; } = string.Empty;
    public List<TypeBien> TypesBien { get; set; }
    public List<StatutBien> StatutsBien { get; set; }
    public TypeBien? TypeBienSelectionne { get; set; }
    public StatutBien? StatutSelectionne { get; set; }
    public List<string> PebOptions { get; } = new() { "A++", "A+", "A", "B", "C", "D", "E", "F", "G" };
    public string? PebSelectionne { get; set; }
    public string? Rue { get; set; }
    public string? Numero { get; set; }
    public string? Boite { get; set; }
    public string? CodePostal { get; set; }
    public string? Commune { get; set; }
    public string? PrixVente { get; set; }
    public string? LoyerMensuel { get; set; }
    public string? ChargesMensuelles { get; set; }
    public string? SurfaceHabitable { get; set; }
    public string? SurfaceTotale { get; set; }
    public string? NombrePieces { get; set; }
    public string? NombreChambres { get; set; }
    public string? NombreSalleBain { get; set; }
    public string? NombreWc { get; set; }
    public string? AnneeConstruction { get; set; }
    public string? Description { get; set; }
    public string? Chauffage { get; set; }
    public string? Isolation { get; set; }
    public bool EstLibre { get; set; }
    public bool Ascenseur { get; set; }
    public bool CuisineEquipee { get; set; }
    public bool Cave { get; set; }
    public bool Grenier { get; set; }
    public bool Garage { get; set; }
    public bool Jardin { get; set; }

    public AjouterBienViewModel()
    {
        TypesBien = BienService.GetTypesBien();
        StatutsBien = BienService.GetStatutsBien();
    }

    private ServiceResult Validate()
    {
        if (string.IsNullOrWhiteSpace(Reference))
            return ServiceResult.Fail("La référence est obligatoire.");

        if (string.IsNullOrWhiteSpace(TitreAnnonce))
            return ServiceResult.Fail("Le titre de l'annonce est obligatoire.");

        if (TypeBienSelectionne == null)
            return ServiceResult.Fail("Veuillez sélectionner un type de bien.");

        if (StatutSelectionne == null)
            return ServiceResult.Fail("Veuillez sélectionner un statut.");

        if (PebSelectionne == null)
            return ServiceResult.Fail("Veuillez sélectionner un PEB.");

        if (string.IsNullOrWhiteSpace(CodePostal))
            return ServiceResult.Fail("Le code postal est obligatoire.");

        if (string.IsNullOrWhiteSpace(Commune))
            return ServiceResult.Fail("La commune est obligatoire.");

        return ServiceResult.Ok();
    }

    public ServiceResult Save()
    {
        var validation = Validate();
        if (!validation.Success)
            return validation;

        var adresse = new Adresse
        {
            Rue = Rue?.Trim(),
            Numero = Numero?.Trim(),
            Boite = Boite?.Trim(),
            CodePostal = CodePostal!.Trim(),
            Commune = Commune!.Trim()
        };

        var bien = new Bien
        {
            Reference = Reference.Trim().ToUpper(),
            TitreAnnonce = TitreAnnonce.Trim(),
            Peb = PebSelectionne!,
            PrixVente = decimal.TryParse(PrixVente, out var pv) ? pv : null,
            LoyerMensuel = decimal.TryParse(LoyerMensuel, out var lm) ? lm : null,
            ChargesMensuelles = decimal.TryParse(ChargesMensuelles, out var cm) ? cm : null,
            SurfaceHabitable = decimal.TryParse(SurfaceHabitable, out var sh) ? sh : null,
            SurfaceTotale = decimal.TryParse(SurfaceTotale, out var st) ? st : null,
            NombrePieces = int.TryParse(NombrePieces, out var np) ? np : null,
            NombreChambres = int.TryParse(NombreChambres, out var nc) ? nc : null,
            NombreSalleBain = int.TryParse(NombreSalleBain, out var nsb) ? nsb : null,
            NombreWc = int.TryParse(NombreWc, out var nwc) ? nwc : null,
            AnneeConstruction = AnneeConstruction?.Trim(),
            Description = Description?.Trim(),
            Chauffage = Chauffage?.Trim(),
            Isolation = Isolation?.Trim(),
            EstLibre = EstLibre,
            Ascenseur = Ascenseur,
            CuisineEquipee = CuisineEquipee,
            Cave = Cave,
            Grenier = Grenier,
            Garage = Garage,
            Jardin = Jardin,
            Adresse = adresse,
            TypeBienId = TypeBienSelectionne!.TypeBienId,
            StatutBienId = StatutSelectionne!.StatutBienId,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };

        return new BienService().AddBien(bien);
    }
}