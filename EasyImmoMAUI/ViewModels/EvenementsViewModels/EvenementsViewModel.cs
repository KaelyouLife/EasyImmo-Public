using BU.Services;
using Common.Utilities;
using DAL.DB;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EasyImmoMAUI.ViewModels;

public class EvenementsViewModel : INotifyPropertyChanged
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

    private ObservableCollection<EvenementRow> _evenements = new();
    public ObservableCollection<EvenementRow> Evenements
    {
        get => _evenements;
        private set => SetField(ref _evenements, value);
    }

    private EvenementRow? _evenementSelectionne;
    public EvenementRow? EvenementSelectionne
    {
        get => _evenementSelectionne;
        set
        {
            SetField(ref _evenementSelectionne, value);
            OnPropertyChanged(nameof(AEvenementSelectionne));
        }
    }

    public bool AEvenementSelectionne => EvenementSelectionne != null;

    private string _recherche = string.Empty;
    public string Recherche
    {
        get => _recherche;
        set
        {
            SetField(ref _recherche, value);
            AppliquerFiltres();
        }
    }

    private TypeEvenement? _typeFiltreSelectionne;
    public TypeEvenement? TypeFiltreSelectionne
    {
        get => _typeFiltreSelectionne;
        set
        {
            SetField(ref _typeFiltreSelectionne, value);
            AppliquerFiltres();
        }
    }

    private bool _formulaireVisible;
    public bool FormulaireVisible
    {
        get => _formulaireVisible;
        set => SetField(ref _formulaireVisible, value);
    }

    private bool _estEnEdition;
    public bool EstEnEdition
    {
        get => _estEnEdition;
        set
        {
            SetField(ref _estEnEdition, value);
            OnPropertyChanged(nameof(TitreFormulaire));
        }
    }

    public string TitreFormulaire => EstEnEdition ? "Modifier l'événement" : "Nouvel événement";

    private string _formDescription = string.Empty;
    public string FormDescription { get => _formDescription; set => SetField(ref _formDescription, value); }

    private string _formDate = string.Empty;
    public string FormDate { get => _formDate; set => SetField(ref _formDate, value); }

    private string _formHeure = string.Empty;
    public string FormHeure { get => _formHeure; set => SetField(ref _formHeure, value); }

    private TypeEvenement? _formTypeSelectionne;
    public TypeEvenement? FormTypeSelectionne { get => _formTypeSelectionne; set => SetField(ref _formTypeSelectionne, value); }

    private Bien? _formBienSelectionne;
    public Bien? FormBienSelectionne { get => _formBienSelectionne; set => SetField(ref _formBienSelectionne, value); }

    private Personne? _formPersonneSelectionnee;
    public Personne? FormPersonneSelectionnee { get => _formPersonneSelectionnee; set => SetField(ref _formPersonneSelectionnee, value); }

    private RoleEvenementPersonne? _formRoleSelectionne;
    public RoleEvenementPersonne? FormRoleSelectionne { get => _formRoleSelectionne; set => SetField(ref _formRoleSelectionne, value); }

    private ObservableCollection<ParticipantRow> _formParticipants = new();
    public ObservableCollection<ParticipantRow> FormParticipants
    {
        get => _formParticipants;
        private set => SetField(ref _formParticipants, value);
    }

    public List<TypeEvenement> TypesEvenement { get; private set; } = new();
    public List<TypeEvenement?> TypesEvenementFiltre { get; private set; } = new();
    public List<Bien> Biens { get; private set; } = new();
    public List<Personne> Personnes { get; private set; } = new();
    public List<RoleEvenementPersonne> RolesEvenement { get; private set; } = new();

    private List<EvenementRow> _tousLesEvenements = new();

    public void Initialiser()
    {
        TypesEvenement = EvenementService.GetTypesEvenement();
        TypesEvenementFiltre = new List<TypeEvenement?> { null }.Concat(TypesEvenement).ToList();
        Biens = BienService.GetBiens();
        Personnes = PersonneService.GetPersonnes();
        RolesEvenement = EvenementService.GetRolesEvenement();

        OnPropertyChanged(nameof(TypesEvenement));
        OnPropertyChanged(nameof(TypesEvenementFiltre));
        OnPropertyChanged(nameof(Biens));
        OnPropertyChanged(nameof(Personnes));
        OnPropertyChanged(nameof(RolesEvenement));

        ChargerEvenements();
    }

    public void ChargerEvenements()
    {
        var liste = EvenementService.GetEvenements();
        _tousLesEvenements = liste.Select(e => new EvenementRow
        {
            EvenementId = e.EvenementId,
            Description = e.Description ?? "—",
            Date = e.DateEvenement.ToString("dd/MM/yyyy"),
            Heure = e.HeureDebut.ToString("HH:mm"),
            TypeLibelle = e.TypeEvenement?.Libelle ?? "—",
            TypeEvenementId = e.TypeEvenementId,
            BienTitre = e.Bien?.TitreAnnonce ?? "—",
            BienCommune = e.Bien?.Adresse?.Commune ?? "",
            EstAccompli = e.EstAccompli ?? false,
            Participants = e.RelationEvenementPersonnes
                .Select(r => new ParticipantRow
                {
                    Nom = $"{r.Personne.Prenom} {r.Personne.Nom}",
                    Role = r.RoleEvenementPersonne?.Libelle ?? "—",
                    Telephone = r.Personne.Telephone ?? "—",
                    Email = r.Personne.Email,
                    Commentaire = r.Commentaire ?? ""
                }).ToList()
        }).ToList();

        AppliquerFiltres();
    }

    private void AppliquerFiltres()
    {
        var filtres = _tousLesEvenements.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(Recherche))
            filtres = filtres.Where(e =>
                e.Description.Contains(Recherche, StringComparison.OrdinalIgnoreCase) ||
                e.BienTitre.Contains(Recherche, StringComparison.OrdinalIgnoreCase) ||
                e.TypeLibelle.Contains(Recherche, StringComparison.OrdinalIgnoreCase));

        if (TypeFiltreSelectionne != null)
            filtres = filtres.Where(e => e.TypeEvenementId == TypeFiltreSelectionne.TypeEvenementId);

        Evenements = new ObservableCollection<EvenementRow>(filtres);
    }

    public void OuvrirFormulaireAjout()
    {
        EstEnEdition = false;
        FormDescription = string.Empty;
        FormDate = DateTime.Today.ToString("dd/MM/yyyy");
        FormHeure = "09:00";
        FormTypeSelectionne = null;
        FormBienSelectionne = null;
        FormPersonneSelectionnee = null;
        FormRoleSelectionne = null;
        FormParticipants = new ObservableCollection<ParticipantRow>();
        FormulaireVisible = true;
    }

    public void OuvrirFormulaireEdition()
    {
        if (EvenementSelectionne == null) return;

        var e = EvenementService.GetEvenementById(EvenementSelectionne.EvenementId);
        if (e == null) return;

        EstEnEdition = true;
        FormDescription = e.Description ?? string.Empty;
        FormDate = e.DateEvenement.ToString("dd/MM/yyyy");
        FormHeure = e.HeureDebut.ToString("HH:mm");
        FormTypeSelectionne = TypesEvenement.FirstOrDefault(t => t.TypeEvenementId == e.TypeEvenementId);
        FormBienSelectionne = Biens.FirstOrDefault(b => b.BienId == e.BienId);
        FormParticipants = new ObservableCollection<ParticipantRow>(
            e.RelationEvenementPersonnes.Select(r => new ParticipantRow
            {
                PersonneId = r.PersonneId,
                RoleEvenementPersonneId = r.RoleEvenementPersonneId,
                Nom = $"{r.Personne.Prenom} {r.Personne.Nom}",
                Role = r.RoleEvenementPersonne?.Libelle ?? "—",
                Telephone = r.Personne.Telephone ?? "—",
                Email = r.Personne.Email,
                Commentaire = r.Commentaire ?? ""
            })
        );
        FormulaireVisible = true;
    }

    public void AjouterParticipant()
    {
        if (FormPersonneSelectionnee == null || FormRoleSelectionne == null) return;

        var dejaPresent = FormParticipants.Any(p => p.Nom == $"{FormPersonneSelectionnee.Prenom} {FormPersonneSelectionnee.Nom}");
        if (dejaPresent) return;

        FormParticipants.Add(new ParticipantRow
        {
            PersonneId = FormPersonneSelectionnee.PersonneId,
            RoleEvenementPersonneId = FormRoleSelectionne.RoleEvenementPersonneId,
            Nom = $"{FormPersonneSelectionnee.Prenom} {FormPersonneSelectionnee.Nom}",
            Role = FormRoleSelectionne.Libelle,
            Telephone = FormPersonneSelectionnee.Telephone ?? "—",
            Email = FormPersonneSelectionnee.Email,
        });

        FormPersonneSelectionnee = null;
        FormRoleSelectionne = null;
    }

    public void RetirerParticipant(ParticipantRow participant)
    {
        FormParticipants.Remove(participant);
    }

    public ServiceResult Enregistrer()
    {
        if (FormTypeSelectionne == null)
            return ServiceResult.Fail("Veuillez sélectionner un type d'événement.");

        if (!DateOnly.TryParseExact(FormDate, "dd/MM/yyyy", out var date))
            return ServiceResult.Fail("Format de date invalide. Utilisez JJ/MM/AAAA.");

        if (!TimeOnly.TryParseExact(FormHeure, "HH:mm", out var heure))
            return ServiceResult.Fail("Format d'heure invalide. Utilisez HH:MM.");

        var evenement = new Evenement
        {
            Description = string.IsNullOrWhiteSpace(FormDescription) ? null : FormDescription.Trim(),
            DateEvenement = date,
            HeureDebut = heure,
            TypeEvenementId = FormTypeSelectionne.TypeEvenementId,
            BienId = FormBienSelectionne?.BienId,
            EstAccompli = false
        };

        var participants = FormParticipants.Select(p => new RelationEvenementPersonne
        {
            PersonneId = p.PersonneId,
            RoleEvenementPersonneId = p.RoleEvenementPersonneId,
            Commentaire = p.Commentaire
        }).ToList();

        ServiceResult result;

        if (EstEnEdition && EvenementSelectionne != null)
        {
            evenement.EvenementId = EvenementSelectionne.EvenementId;
            result = EvenementService.UpdateEvenement(evenement, participants);
        }
        else
        {
            result = EvenementService.AddEvenement(evenement, participants);
        }

        if (result.Success)
        {
            FormulaireVisible = false;
            ChargerEvenements();
        }

        return result;
    }

    public ServiceResult Supprimer()
    {
        if (EvenementSelectionne == null)
            return ServiceResult.Fail("Aucun événement sélectionné.");

        var result = EvenementService.DeleteEvenement(EvenementSelectionne.EvenementId);
        if (result.Success)
        {
            EvenementSelectionne = null;
            ChargerEvenements();
        }
        return result;
    }

    public class EvenementRow
    {
        public int EvenementId { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public string Heure { get; set; } = string.Empty;
        public string TypeLibelle { get; set; } = string.Empty;
        public int TypeEvenementId { get; set; }
        public string BienTitre { get; set; } = string.Empty;
        public string BienCommune { get; set; } = string.Empty;
        public bool EstAccompli { get; set; }
        public List<ParticipantRow> Participants { get; set; } = new();
        public string ParticipantsFormates => Participants.Count > 0
            ? string.Join(", ", Participants.Select(p => p.Nom))
            : "Aucun participant";
    }

    public class ParticipantRow
    {
        public int PersonneId { get; set; }
        public int RoleEvenementPersonneId { get; set; }
        public string Nom { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Telephone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Commentaire { get; set; } = string.Empty;
    }
}