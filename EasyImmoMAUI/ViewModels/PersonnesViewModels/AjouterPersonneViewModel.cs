using BU.Services;
using Common.Utilities;
using DAL.DB;

namespace EasyImmoMAUI.ViewModels.PersonnesViewModels;

public class AjouterPersonneViewModel
{
    public string Prenom { get; set; } = string.Empty;
    public string Nom { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Telephone { get; set; }
    public List<string> SexeOptions { get; } = new() { "M", "F", "X" };
    public string? SexeSelectionne { get; set; }
    public string? DateNaissance { get; set; }
    public string? CompteBancaire { get; set; }

    public string? Rue { get; set; }
    public string? Numero { get; set; }
    public string? Boite { get; set; }
    public string? CodePostal { get; set; }
    public string? Commune { get; set; }

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
            Prenom = Prenom.Trim(),
            Nom = Nom.Trim(),
            Email = Email.Trim(),
            Telephone = Telephone?.Trim(),
            Sexe = SexeSelectionne!,
            DateNaissance = dateNaissance,
            CompteBancaire = CompteBancaire?.Trim(),
            Adresse = adresse,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };

        return PersonneService.AddPersonne(personne);
    }
}