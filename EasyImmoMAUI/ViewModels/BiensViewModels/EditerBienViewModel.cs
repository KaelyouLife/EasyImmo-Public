using BU.Services;
using Common;
using Common.Utilities;
using DAL.DB;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EasyImmoMAUI.ViewModels.BiensViewModels;

public class EditerBienViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    private bool SetField<T>(ref T field, T value, [CallerMemberName] string? name = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(name);
        return true;
    }

    private int _bienId;
    private string _reference = string.Empty;
    private string _titreAnnonce = string.Empty;
    private TypeBien? _typeBienSelectionne;
    private StatutBien? _statutSelectionne;
    private string? _pebSelectionne;
    private string? _rue;
    private string? _numero;
    private string? _boite;
    private string? _codePostal;
    private string? _commune;
    private string? _prixVente;
    private string? _loyerMensuel;
    private string? _chargesMensuelles;
    private string? _surfaceHabitable;
    private string? _surfaceTotale;
    private string? _nombrePieces;
    private string? _nombreChambres;
    private string? _nombreSalleBain;
    private string? _nombreWc;
    private string? _anneeConstruction;
    private string? _description;
    private string? _chauffage;
    private string? _isolation;
    private bool _estLibre;
    private bool _ascenseur;
    private bool _cuisineEquipee;
    private bool _cave;
    private bool _grenier;
    private bool _garage;
    private bool _jardin;

    public int BienId { get => _bienId; private set => SetField(ref _bienId, value); }
    public string Reference { get => _reference; set => SetField(ref _reference, value); }
    public string TitreAnnonce { get => _titreAnnonce; set => SetField(ref _titreAnnonce, value); }
    public List<TypeBien> TypesBien { get; set; }
    public List<StatutBien> StatutsBien { get; set; }
    public List<string> PebOptions { get; } = new() { "A++", "A+", "A", "B", "C", "D", "E", "F", "G" };
    public TypeBien? TypeBienSelectionne { get => _typeBienSelectionne; set => SetField(ref _typeBienSelectionne, value); }
    public StatutBien? StatutSelectionne { get => _statutSelectionne; set => SetField(ref _statutSelectionne, value); }
    public string? PebSelectionne { get => _pebSelectionne; set => SetField(ref _pebSelectionne, value); }
    public int AdresseId { get; private set; }
    public string? Rue { get => _rue; set => SetField(ref _rue, value); }
    public string? Numero { get => _numero; set => SetField(ref _numero, value); }
    public string? Boite { get => _boite; set => SetField(ref _boite, value); }
    public string? CodePostal { get => _codePostal; set => SetField(ref _codePostal, value); }
    public string? Commune { get => _commune; set => SetField(ref _commune, value); }
    public string? PrixVente { get => _prixVente; set => SetField(ref _prixVente, value); }
    public string? LoyerMensuel { get => _loyerMensuel; set => SetField(ref _loyerMensuel, value); }
    public string? ChargesMensuelles { get => _chargesMensuelles; set => SetField(ref _chargesMensuelles, value); }
    public string? SurfaceHabitable { get => _surfaceHabitable; set => SetField(ref _surfaceHabitable, value); }
    public string? SurfaceTotale { get => _surfaceTotale; set => SetField(ref _surfaceTotale, value); }
    public string? NombrePieces { get => _nombrePieces; set => SetField(ref _nombrePieces, value); }
    public string? NombreChambres { get => _nombreChambres; set => SetField(ref _nombreChambres, value); }
    public string? NombreSalleBain { get => _nombreSalleBain; set => SetField(ref _nombreSalleBain, value); }
    public string? NombreWc { get => _nombreWc; set => SetField(ref _nombreWc, value); }
    public string? AnneeConstruction { get => _anneeConstruction; set => SetField(ref _anneeConstruction, value); }
    public string? Description { get => _description; set => SetField(ref _description, value); }
    public string? Chauffage { get => _chauffage; set => SetField(ref _chauffage, value); }
    public string? Isolation { get => _isolation; set => SetField(ref _isolation, value); }
    public bool EstLibre { get => _estLibre; set => SetField(ref _estLibre, value); }
    public bool Ascenseur { get => _ascenseur; set => SetField(ref _ascenseur, value); }
    public bool CuisineEquipee { get => _cuisineEquipee; set => SetField(ref _cuisineEquipee, value); }
    public bool Cave { get => _cave; set => SetField(ref _cave, value); }
    public bool Grenier { get => _grenier; set => SetField(ref _grenier, value); }
    public bool Garage { get => _garage; set => SetField(ref _garage, value); }
    public bool Jardin { get => _jardin; set => SetField(ref _jardin, value); }

    public EditerBienViewModel()
    {
        TypesBien = BienService.GetTypesBien();
        StatutsBien = BienService.GetStatutsBien();
    }

    public void ChargerBien(int bienId)
    {
        var bien = BienService.GetBienById(bienId);
        AdresseId = bien.Adresse?.AdresseId ?? 0;
        if (bien == null) return;

        BienId = bien.BienId;

        Reference = bien.Reference;
        TitreAnnonce = bien.TitreAnnonce;
        PebSelectionne = PebOptions.FirstOrDefault(p => p == bien.Peb);
        TypeBienSelectionne = TypesBien.FirstOrDefault(t => t.TypeBienId == bien.TypeBienId);
        StatutSelectionne = StatutsBien.FirstOrDefault(s => s.StatutBienId == bien.StatutBienId);

        Rue = bien.Adresse?.Rue;
        Numero = bien.Adresse?.Numero;
        Boite = bien.Adresse?.Boite;
        CodePostal = bien.Adresse?.CodePostal;
        Commune = bien.Adresse?.Commune;

        PrixVente = bien.PrixVente?.ToString();
        LoyerMensuel = bien.LoyerMensuel?.ToString();
        ChargesMensuelles = bien.ChargesMensuelles?.ToString();

        SurfaceHabitable = bien.SurfaceHabitable?.ToString();
        SurfaceTotale = bien.SurfaceTotale?.ToString();
        NombrePieces = bien.NombrePieces?.ToString();
        NombreChambres = bien.NombreChambres?.ToString();
        NombreSalleBain = bien.NombreSalleBain?.ToString();
        NombreWc = bien.NombreWc?.ToString();
        AnneeConstruction = bien.AnneeConstruction;

        Description = bien.Description;
        Chauffage = bien.Chauffage;
        Isolation = bien.Isolation;

        EstLibre = bien.EstLibre ?? false;
        Ascenseur = bien.Ascenseur ?? false;
        CuisineEquipee = bien.CuisineEquipee ?? false;
        Cave = bien.Cave ?? false;
        Grenier = bien.Grenier ?? false;
        Garage = bien.Garage ?? false;
        Jardin = bien.Jardin ?? false;
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
            AdresseId = AdresseId,
            Rue = Rue?.Trim(),
            Numero = Numero?.Trim(),
            Boite = Boite?.Trim(),
            CodePostal = CodePostal!.Trim(),
            Commune = Commune!.Trim()
        };

        var bien = new Bien
        {
            BienId = BienId,
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
        };

        return new BienService().UpdateBien(bien);
    }
}