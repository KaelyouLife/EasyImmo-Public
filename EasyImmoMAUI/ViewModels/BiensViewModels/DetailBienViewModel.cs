using BU.Services;
using DAL.DB;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EasyImmoMAUI.ViewModels.BiensViewModels;

public class DetailBienViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public DocumentsBienViewModel DocumentsViewModel { get; } = new();
    public EvenementsBienViewModel EvenementsViewModel { get; } = new();
    public ContactsBienViewModel ContactsViewModel { get; } = new();
    public PhotosBienViewModel PhotosViewModel { get; } = new();

    private ObservableCollection<HistoriqueStatutBien> _historique = new();

    public ObservableCollection<HistoriqueStatutBien> Historique
    {
        get => _historique;
        private set
        {
            _historique = value;
            OnPropertyChanged();
        }
    }

    private Bien? _bien;
    public Bien? Bien
    {
        get => _bien;
        set
        {
            _bien = value;
            OnPropertyChanged();
            RefreshComputedProperties();
        }
    }

    private bool _isLoading;
    public bool IsLoading
    {
        get => _isLoading;
        set
        {
            _isLoading = value;
            OnPropertyChanged();
        }
    }

    public DetailBienViewModel()
    {
    }

    public async Task ChargerBienAsync(int bienId)
    {
        IsLoading = true;
        var historique = BienService.GetHistoriqueByBienId(bienId);
        Historique = new ObservableCollection<HistoriqueStatutBien>(historique);

        await Task.Run(() =>
        {
            Bien = BienService.GetBienById(bienId);
            DocumentsViewModel.Initialiser(bienId);
            EvenementsViewModel.Initialiser(bienId);
            ContactsViewModel.Initialiser(bienId);
            PhotosViewModel.Initialiser(bienId);
        });

        IsLoading = false;
    }

    public string PrixFormate =>
        Bien?.PrixVente.HasValue == true
            ? $"{Bien.PrixVente.Value:N0} €"
            : "—";

    public string PebTexte =>
        !string.IsNullOrWhiteSpace(Bien?.Peb)
            ? $"PEB {Bien.Peb}"
            : "PEB —";

    public string SurfaceTotaleTexte =>
        Bien?.SurfaceTotale.HasValue == true
            ? $"{Bien.SurfaceTotale.Value} m²"
            : "—";

    public string SurfaceHabitableTexte =>
        Bien?.SurfaceHabitable.HasValue == true
            ? $"{Bien.SurfaceHabitable.Value} m²"
            : "—";

    public string ChambresTexte =>
        Bien?.NombreChambres.HasValue == true
            ? $"{Bien.NombreChambres.Value} chambre(s)"
            : "—";

    public bool HasChambres => Bien?.NombreChambres.HasValue == true;
    public bool HasSurface => Bien?.SurfaceHabitable.HasValue == true;

    public string PiecesTexte =>
        Bien?.NombrePieces.HasValue == true
            ? $"{Bien.NombrePieces.Value} pièce(s)"
            : "—";

    public string SalleBainTexte =>
        Bien?.NombreSalleBain.HasValue == true
            ? $"{Bien.NombreSalleBain.Value} salle(s) de bain"
            : "—";

    public string WcTexte =>
        Bien?.NombreWc.HasValue == true
            ? $"{Bien.NombreWc.Value} WC"
            : "—";

    public string CuisineTexte =>
        Bien?.CuisineEquipee == true
            ? "Cuisine équipée"
            : "Cuisine non équipée";

    public string GarageTexte =>
        Bien?.Garage == true
            ? "Garage"
            : "Pas de garage";

    public string CaveTexte =>
        Bien?.Cave == true
            ? "Cave"
            : "Pas de cave";

    public string GrenierTexte =>
        Bien?.Grenier == true
            ? "Grenier"
            : "Pas de grenier";

    public string IsolationTexte =>
        !string.IsNullOrWhiteSpace(Bien?.Isolation)
            ? Bien.Isolation
            : "Isolation non précisée";

    public string AdresseFormatee
    {
        get
        {
            var a = Bien?.Adresse;

            if (a == null)
                return "—";

            return $"{a.Rue} {a.Numero}\n{a.CodePostal} {a.Commune}\nBelgique";
        }
    }

    public string PhotoPrincipale
    {
        get
        {
            var photo = Bien?.PhotoBiens?
                .FirstOrDefault(p => p.EstPrincipale == true)
                ?? Bien?.PhotoBiens?.FirstOrDefault();

            return photo != null
                ? photo.Chemin
                : "placeholder_house.png";
        }
    }

    private void RefreshComputedProperties()
    {
        OnPropertyChanged(nameof(PrixFormate));
        OnPropertyChanged(nameof(PebTexte));
        OnPropertyChanged(nameof(SurfaceTotaleTexte));
        OnPropertyChanged(nameof(SurfaceHabitableTexte));
        OnPropertyChanged(nameof(ChambresTexte));
        OnPropertyChanged(nameof(HasChambres));
        OnPropertyChanged(nameof(HasSurface));
        OnPropertyChanged(nameof(PiecesTexte));
        OnPropertyChanged(nameof(SalleBainTexte));
        OnPropertyChanged(nameof(WcTexte));
        OnPropertyChanged(nameof(CuisineTexte));
        OnPropertyChanged(nameof(GarageTexte));
        OnPropertyChanged(nameof(CaveTexte));
        OnPropertyChanged(nameof(GrenierTexte));
        OnPropertyChanged(nameof(IsolationTexte));
        OnPropertyChanged(nameof(AdresseFormatee));
        OnPropertyChanged(nameof(PhotoPrincipale));
    }

    protected void OnPropertyChanged([CallerMemberName] string? name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}