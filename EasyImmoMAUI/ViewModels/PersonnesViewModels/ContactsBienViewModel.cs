using BU.Services;
using Common.Utilities;
using DAL.DB;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EasyImmoMAUI.ViewModels.BiensViewModels;

public class ContactsBienViewModel : INotifyPropertyChanged
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
    private string _recherche = string.Empty;
    private ObservableCollection<ContactRow> _contacts = new();
    private ObservableCollection<ContactRow> _contactsFiltres = new();

    private List<Personne> _personnesDisponibles = new();
    private Personne? _personneSelectionnee;
    private string _roleSelectionne = string.Empty;

    public ObservableCollection<ContactRow> ContactsFiltres
    {
        get => _contactsFiltres;
        private set => SetField(ref _contactsFiltres, value);
    }

    public string Recherche
    {
        get => _recherche;
        set
        {
            SetField(ref _recherche, value);
            AppliquerFiltres();
        }
    }

    public List<Personne> PersonnesDisponibles
    {
        get => _personnesDisponibles;
        private set => SetField(ref _personnesDisponibles, value);
    }

    public Personne? PersonneSelectionnee
    {
        get => _personneSelectionnee;
        set => SetField(ref _personneSelectionnee, value);
    }

    public string RoleSelectionne
    {
        get => _roleSelectionne;
        set => SetField(ref _roleSelectionne, value);
    }

    public List<string> RolesDisponibles { get; } = new()
    {
        "Intéressé", "Acheteur", "Vendeur", "Locataire", "Propriétaire",
        "Expert", "Notaire", "Agent", "Autre"
    };

    public void Initialiser(int bienId)
    {
        _bienId = bienId;
        ChargerContacts();
    }

    public void ChargerContacts()
    {
        var relations = PersonneService.GetContactsByBienId(_bienId);
        var listeIds = relations.Select(r => r.PersonneId).ToHashSet();

        _contacts = new ObservableCollection<ContactRow>(
            relations.Select(r => new ContactRow
            {
                RelationId = r.RelationBienPersonneId,
                NomComplet = $"{r.Personne.Prenom} {r.Personne.Nom}",
                Role = r.Role,
                Telephone = r.Personne.Telephone ?? "—",
                Email = r.Personne.Email
            })
        );

        var toutes = PersonneService.GetPersonnes();
        PersonnesDisponibles = toutes.Where(p => !listeIds.Contains(p.PersonneId)).ToList();

        AppliquerFiltres();
    }

    private void AppliquerFiltres()
    {
        var filtres = _contacts.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(Recherche))
            filtres = filtres.Where(c =>
                c.NomComplet.Contains(Recherche, StringComparison.OrdinalIgnoreCase) ||
                c.Role.Contains(Recherche, StringComparison.OrdinalIgnoreCase));

        ContactsFiltres = new ObservableCollection<ContactRow>(filtres);
    }

    public ServiceResult AjouterContact()
    {
        if (PersonneSelectionnee == null)
            return ServiceResult.Fail("Veuillez sélectionner une personne.");

        if (string.IsNullOrWhiteSpace(RoleSelectionne))
            return ServiceResult.Fail("Veuillez sélectionner un rôle.");

        var result = PersonneService.AddContactBien(
            bienId: _bienId,
            personneId: PersonneSelectionnee.PersonneId,
            role: RoleSelectionne);

        if (result.Success)
        {
            PersonneSelectionnee = null;
            RoleSelectionne = string.Empty;
            ChargerContacts();
        }

        return result;
    }
    public ServiceResult SupprimerContact(int relationId)
    {
        var result = PersonneService.RemoveContactBien(relationId);
        if (result.Success)
            ChargerContacts();
        return result;
    }

    public class ContactRow
    {
        public int RelationId { get; set; }
        public string NomComplet { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Telephone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Contact => $"{Telephone} - {Email}";
    }
}