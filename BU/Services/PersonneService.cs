using Common.Utilities;
using DAL.DB;
using Microsoft.EntityFrameworkCore;

namespace BU.Services;

public class PersonneService
{
    public static List<Personne> GetPersonnes()
    {
        using var db = new EasyImmoContext();
        return db.Personnes
            .Include(p => p.Adresse)
            .OrderBy(p => p.Nom)
            .ThenBy(p => p.Prenom)
            .ToList();
    }

    public static Personne? GetPersonneById(int id)
    {
        using var db = new EasyImmoContext();
        return db.Personnes
            .Include(p => p.Adresse)
            .FirstOrDefault(p => p.PersonneId == id);
    }

    public static List<RelationBienPersonne> GetContactsByBienId(int bienId)
    {
        using var db = new EasyImmoContext();
        return db.RelationBienPersonnes
            .Where(r => r.BienId == bienId)
            .Include(r => r.Personne)
            .OrderBy(r => r.Personne.Nom)
            .ToList();
    }

    public static ServiceResult AddContactBien(int bienId, int personneId, string role)
    {
        try
        {
            using var db = new EasyImmoContext();

            var existe = db.RelationBienPersonnes.Any(r =>
                r.BienId == bienId && r.PersonneId == personneId);

            if (existe)
                return ServiceResult.Fail("Cette personne est déjà liée à ce bien.");

            db.RelationBienPersonnes.Add(new RelationBienPersonne
            {
                BienId = bienId,
                PersonneId = personneId,
                Role = role
            });

            db.SaveChanges();
            return ServiceResult.Ok();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.ToString());
            return ServiceResult.Fail("Impossible d'ajouter le contact.");
        }
    }

    public static ServiceResult RemoveContactBien(int relationBienPersonneId)
    {
        try
        {
            using var db = new EasyImmoContext();

            var relation = db.RelationBienPersonnes.Find(relationBienPersonneId);
            if (relation == null)
                return ServiceResult.Fail("Relation introuvable.");

            db.RelationBienPersonnes.Remove(relation);
            db.SaveChanges();
            return ServiceResult.Ok();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.ToString());
            return ServiceResult.Fail("Impossible de supprimer le contact.");
        }
    }

    // Retourne toutes les personnes avec toutes leurs relations
    public static List<Personne> GetPersonnesAvecRelations()
    {
        using var db = new EasyImmoContext();
        return db.Personnes
            .Include(p => p.Adresse)
            .Include(p => p.RelationBienPersonnes)
                .ThenInclude(r => r.Bien)
                    .ThenInclude(b => b.Adresse)
            .Include(p => p.RelationEvenementPersonnes)
                .ThenInclude(r => r.Evenement)
                    .ThenInclude(e => e.TypeEvenement)
            .Include(p => p.RelationEvenementPersonnes)
                .ThenInclude(r => r.Evenement)
                    .ThenInclude(e => e.Bien)
            .OrderBy(p => p.Nom)
            .ThenBy(p => p.Prenom)
            .ToList();
    }

    public static ServiceResult DeletePersonne(int personneId)
    {
        try
        {
            using var db = new EasyImmoContext();

            var aDesTransactions = db.Transactions.Any(t =>
                t.BeneficiaireId == personneId || t.PayeurId == personneId);

            if (aDesTransactions)
                return ServiceResult.Fail("Cette personne ne peut pas être supprimée car elle est liée à des transactions.");

            var personne = db.Personnes.Find(personneId);
            if (personne == null)
                return ServiceResult.Fail("Personne introuvable.");

            db.Personnes.Remove(personne);
            db.SaveChanges();
            return ServiceResult.Ok();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.ToString());
            return ServiceResult.Fail("Impossible de supprimer la personne.");
        }
    }

    public static ServiceResult AddPersonne(Personne personne)
    {
        try
        {
            using var db = new EasyImmoContext();
            if (personne.Adresse != null)
                db.Adresses.Add(personne.Adresse);
            db.Personnes.Add(personne);
            db.SaveChanges();
            return ServiceResult.Ok();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.ToString());
            return ServiceResult.Fail("Impossible d'ajouter la personne.");
        }
    }

    public static ServiceResult UpdatePersonne(Personne personne)
    {
        try
        {
            using var db = new EasyImmoContext();

            var existing = db.Personnes.Include(p => p.Adresse)
                .FirstOrDefault(p => p.PersonneId == personne.PersonneId);
            if (existing == null)
                return ServiceResult.Fail("Personne introuvable.");

            var createdAt = existing.CreatedAt;
            var adresseId = existing.AdresseId;

            db.Entry(existing).CurrentValues.SetValues(personne);
            existing.CreatedAt = createdAt;
            existing.AdresseId = adresseId;
            existing.UpdatedAt = DateTime.Now;

            if (existing.Adresse != null && personne.Adresse != null)
            {
                existing.Adresse.Rue = personne.Adresse.Rue;
                existing.Adresse.Numero = personne.Adresse.Numero;
                existing.Adresse.Boite = personne.Adresse.Boite;
                existing.Adresse.CodePostal = personne.Adresse.CodePostal;
                existing.Adresse.Commune = personne.Adresse.Commune;
            }

            db.SaveChanges();
            return ServiceResult.Ok();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.ToString());
            return ServiceResult.Fail("Impossible de mettre à jour la personne.");
        }
    }
}