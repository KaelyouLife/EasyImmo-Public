using BU.Services;
using Common.Utilities;
using DAL.DB;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EasyImmoMAUI.ViewModels.PersonnesViewModels;

public class PersonnesPageViewModel : INotifyPropertyChanged
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

    private List<Personne> _toutesLesPersonnes = new();

    private ObservableCollection<Personne> _personnes = new();
    public ObservableCollection<Personne> Personnes
    {
        get => _personnes;
        private set => SetField(ref _personnes, value);
    }

    private Personne? _personneSelectionnee;
    public Personne? PersonneSelectionnee
    {
        get => _personneSelectionnee;
        set
        {
            SetField(ref _personneSelectionnee, value);
            OnPropertyChanged(nameof(APersonneSelectionnee));
            OnPropertyChanged(nameof(BiensDeLaPersonne));
            OnPropertyChanged(nameof(DerniersEvenements));
            OnPropertyChanged(nameof(AdresseFormatee));
        }
    }

    public bool APersonneSelectionnee => PersonneSelectionnee != null;

    private string _recherche = string.Empty;
    public string Recherche
    {
        get => _recherche;
        set { SetField(ref _recherche, value); AppliquerFiltres(); }
    }
    public List<RelationBienPersonne> BiensDeLaPersonne =>
        PersonneSelectionnee?.RelationBienPersonnes?.ToList() ?? new();

    public List<Evenement> DerniersEvenements =>
        PersonneSelectionnee?.RelationEvenementPersonnes?
            .Select(r => r.Evenement)
            .Where(e => e != null)
            .OrderByDescending(e => e.DateEvenement)
            .Take(5)
            .ToList() ?? new();

    public string AdresseFormatee
    {
        get
        {
            var a = PersonneSelectionnee?.Adresse;
            if (a == null) return "Adresse non renseignée";
            var parts = new List<string>();
            if (!string.IsNullOrWhiteSpace(a.Rue))
                parts.Add($"{a.Rue} {a.Numero}".Trim());
            if (!string.IsNullOrWhiteSpace(a.CodePostal) || !string.IsNullOrWhiteSpace(a.Commune))
                parts.Add($"{a.CodePostal} {a.Commune}".Trim());
            return parts.Count > 0 ? string.Join(" - ", parts) : "Adresse non renseignée";
        }
    }

    private List<Bien> _biensDisponibles = new();
    public List<Bien> BiensDisponibles
    {
        get => _biensDisponibles;
        private set => SetField(ref _biensDisponibles, value);
    }

    private Bien? _bienALierSelectionne;
    public Bien? BienALierSelectionne
    {
        get => _bienALierSelectionne;
        set => SetField(ref _bienALierSelectionne, value);
    }

    private string _roleALier = string.Empty;
    public string RoleALier
    {
        get => _roleALier;
        set => SetField(ref _roleALier, value);
    }

    public List<string> RolesDisponibles { get; } = new()
    {
        "Intéressé", "Acheteur", "Vendeur", "Locataire",
        "Propriétaire", "Expert", "Notaire", "Agent", "Autre"
    };
    public void LoadPersonnes()
    {
        _toutesLesPersonnes = PersonneService.GetPersonnesAvecRelations();
        AppliquerFiltres();
        MettreAJourBiensDisponibles();
    }

    private void AppliquerFiltres()
    {
        var filtres = _toutesLesPersonnes.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(Recherche))
            filtres = filtres.Where(p =>
                p.Nom.Contains(Recherche, StringComparison.OrdinalIgnoreCase) ||
                p.Prenom.Contains(Recherche, StringComparison.OrdinalIgnoreCase) ||
                (p.Telephone != null && p.Telephone.Contains(Recherche)));

        Personnes = new ObservableCollection<Personne>(filtres);
    }

    private void MettreAJourBiensDisponibles()
    {
        if (PersonneSelectionnee == null)
        {
            BiensDisponibles = BienService.GetBiens();
            return;
        }

        var biensLies = PersonneSelectionnee.RelationBienPersonnes
            .Select(r => r.BienId).ToHashSet();

        BiensDisponibles = BienService.GetBiens()
            .Where(b => !biensLies.Contains(b.BienId))
            .ToList();
    }

    public ServiceResult AjouterBienLie()
    {
        if (PersonneSelectionnee == null)
            return ServiceResult.Fail("Aucune personne sélectionnée.");
        if (BienALierSelectionne == null)
            return ServiceResult.Fail("Veuillez sélectionner un bien.");
        if (string.IsNullOrWhiteSpace(RoleALier))
            return ServiceResult.Fail("Veuillez sélectionner un rôle.");

        var result = PersonneService.AddContactBien(
            BienALierSelectionne.BienId,
            PersonneSelectionnee.PersonneId,
            RoleALier);

        if (result.Success)
        {
            BienALierSelectionne = null;
            RoleALier = string.Empty;
            LoadPersonnes();

            PersonneSelectionnee = _toutesLesPersonnes
                .FirstOrDefault(p => p.PersonneId == PersonneSelectionnee?.PersonneId);
        }

        return result;
    }

    public ServiceResult SupprimerBienLie(int relationBienPersonneId)
    {
        var result = PersonneService.RemoveContactBien(relationBienPersonneId);
        if (result.Success)
        {
            var id = PersonneSelectionnee?.PersonneId;
            LoadPersonnes();
            PersonneSelectionnee = _toutesLesPersonnes.FirstOrDefault(p => p.PersonneId == id);
        }
        return result;
    }

    public ServiceResult SupprimerPersonne()
    {
        if (PersonneSelectionnee == null)
            return ServiceResult.Fail("Aucune personne sélectionnée.");

        var result = PersonneService.DeletePersonne(PersonneSelectionnee.PersonneId);
        if (result.Success)
        {
            PersonneSelectionnee = null;
            LoadPersonnes();
        }
        return result;
    }
}