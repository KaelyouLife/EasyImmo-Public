using BU.Services;
using Common.Utilities;
using DAL.DB;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EasyImmoMAUI.ViewModels.PersonnesViewModels;

public class EditerPersonneViewModel : INotifyPropertyChanged
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

    private int _personneId;
    private int? _adresseId;
    private string _prenom = string.Empty;
    private string _nom = string.Empty;
    private string _email = string.Empty;
    private string? _telephone;
    private string? _sexeSelectionne;
    private string? _dateNaissance;
    private string? _compteBancaire;
    private string? _rue;
    private string? _numero;
    private string? _boite;
    private string? _codePostal;
    private string? _commune;

    public int PersonneId { get => _personneId; private set => SetField(ref _personneId, value); }
    public string Prenom { get => _prenom; set => SetField(ref _prenom, value); }
    public string Nom { get => _nom; set => SetField(ref _nom, value); }
    public string Email { get => _email; set => SetField(ref _email, value); }
    public string? Telephone { get => _telephone; set => SetField(ref _telephone, value); }
    public string? SexeSelectionne { get => _sexeSelectionne; set => SetField(ref _sexeSelectionne, value); }
    public string? DateNaissance { get => _dateNaissance; set => SetField(ref _dateNaissance, value); }
    public string? CompteBancaire { get => _compteBancaire; set => SetField(ref _compteBancaire, value); }
    public string? Rue { get => _rue; set => SetField(ref _rue, value); }
    public string? Numero { get => _numero; set => SetField(ref _numero, value); }
    public string? Boite { get => _boite; set => SetField(ref _boite, value); }
    public string? CodePostal { get => _codePostal; set => SetField(ref _codePostal, value); }
    public string? Commune { get => _commune; set => SetField(ref _commune, value); }

    public List<string> SexeOptions { get; } = new() { "M", "F", "X" };
    public void ChargerPersonne(int personneId)
    {
        var p = PersonneService.GetPersonneById(personneId);
        if (p == null) return;

        PersonneId = p.PersonneId;
        _adresseId = p.AdresseId;

        Prenom = p.Prenom;
        Nom = p.Nom;
        Email = p.Email;
        Telephone = p.Telephone;
        SexeSelectionne = p.Sexe;
        DateNaissance = p.DateNaissance?.ToString("dd/MM/yyyy");
        CompteBancaire = p.CompteBancaire;

        Rue = p.Adresse?.Rue;
        Numero = p.Adresse?.Numero;
        Boite = p.Adresse?.Boite;
        CodePostal = p.Adresse?.CodePostal;
        Commune = p.Adresse?.Commune;
    }

    private ServiceResult Validate()
    {
        if (string.IsNullOrWhiteSpace(Prenom))
            return ServiceResult.Fail("Le prénom est obligatoire.");
        if (string.IsNullOrWhiteSpace(Nom))
            return ServiceResult.Fail("Le nom est obligatoire.");
        if (string.IsNullOrWhiteSpace(Email))
            return ServiceResult.Fail("L'email est obligatoire.");
        if (string.IsNullOrWhiteSpace(SexeSelectionne))
            return ServiceResult.Fail("Veuillez sélectionner un sexe.");
        return ServiceResult.Ok();
    }

    public ServiceResult Save()
    {
        var validation = Validate();
        if (!validation.Success) return validation;

        Adresse? adresse = null;
        if (!string.IsNullOrWhiteSpace(Rue) || !string.IsNullOrWhiteSpace(Commune))
        {
            adresse = new Adresse
            {
                Rue = Rue?.Trim(),
                Numero = Numero?.Trim(),
                Boite = Boite?.Trim(),
                CodePostal = CodePostal?.Trim(),
                Commune = Commune?.Trim()
            };
        }

        DateOnly? dateNaissance = null;
        if (!string.IsNullOrWhiteSpace(DateNaissance) &&
            DateOnly.TryParseExact(DateNaissance, "dd/MM/yyyy", out var d))
            dateNaissance = d;

        var personne = new Personne
        {
            PersonneId = PersonneId,
            Prenom = Prenom.Trim(),
            Nom = Nom.Trim(),
            Email = Email.Trim(),
            Telephone = Telephone?.Trim(),
            Sexe = SexeSelectionne!,
            DateNaissance = dateNaissance,
            CompteBancaire = CompteBancaire?.Trim(),
            AdresseId = _adresseId ?? 0,
            Adresse = adresse,
            UpdatedAt = DateTime.Now
        };

        return PersonneService.UpdatePersonne(personne);
    }
}