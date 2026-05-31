using Common.Utilities;
using DAL.DB;
using Microsoft.EntityFrameworkCore;

namespace BU.Services;

public class EvenementService
{
    public static List<Evenement> GetEvenements()
    {
        using var db = new EasyImmoContext();
        return db.Evenements
            .Include(e => e.TypeEvenement)
            .Include(e => e.RelationEvenementPersonnes)
                .ThenInclude(r => r.Personne)
            .Include(e => e.RelationEvenementPersonnes)
                .ThenInclude(r => r.RoleEvenementPersonne)
            .Include(e => e.Bien)
                .ThenInclude(b => b.Adresse)
            .OrderByDescending(e => e.DateEvenement)
            .ThenBy(e => e.HeureDebut)
            .ToList();
    }

    public static Evenement? GetEvenementById(int id)
    {
        using var db = new EasyImmoContext();
        return db.Evenements
            .Include(e => e.TypeEvenement)
            .Include(e => e.RelationEvenementPersonnes)
                .ThenInclude(r => r.Personne)
            .Include(e => e.RelationEvenementPersonnes)
                .ThenInclude(r => r.RoleEvenementPersonne)
            .Include(e => e.Bien)
                .ThenInclude(b => b.Adresse)
            .FirstOrDefault(e => e.EvenementId == id);
    }

    public static List<Evenement> GetEvenementsByBienId(int bienId)
    {
        using var db = new EasyImmoContext();
        return db.Evenements
            .Where(e => e.BienId == bienId)
            .Include(e => e.TypeEvenement)
            .Include(e => e.RelationEvenementPersonnes)
                .ThenInclude(r => r.Personne)
            .OrderByDescending(e => e.DateEvenement)
            .ToList();
    }

    // Retourne les événements de la semaine en cours.
    public static List<Evenement> GetEvenementsByWeek()
    {
        using var db = new EasyImmoContext();

        var aujourdhui = DateOnly.FromDateTime(DateTime.Today);
        var debutSemaine = aujourdhui.AddDays(-(int)aujourdhui.DayOfWeek + 1); // lundi
        var finSemaine = debutSemaine.AddDays(6); // dimanche

        return db.Evenements
            .Where(e => e.DateEvenement >= debutSemaine && e.DateEvenement <= finSemaine)
            .Include(e => e.TypeEvenement)
            .Include(e => e.RelationEvenementPersonnes)
                .ThenInclude(r => r.Personne)
            .Include(e => e.Bien)
            .OrderBy(e => e.DateEvenement)
            .ThenBy(e => e.HeureDebut)
            .ToList();
    }


    // Retourne les événements d'aujourd'hui.
    public static List<Evenement> GetEvenementsToday()
    {
        using var db = new EasyImmoContext();

        var aujourdhui = DateOnly.FromDateTime(DateTime.Today);

        return db.Evenements
            .Where(e => e.DateEvenement == aujourdhui)
            .Include(e => e.TypeEvenement)
            .Include(e => e.RelationEvenementPersonnes)
                .ThenInclude(r => r.Personne)
            .Include(e => e.Bien)
            .OrderBy(e => e.HeureDebut)
            .ToList();
    }

    public static List<TypeEvenement> GetTypesEvenement()
    {
        using var db = new EasyImmoContext();
        return db.TypeEvenements.OrderBy(t => t.Libelle).ToList();
    }

    public static ServiceResult AddEvenement(Evenement evenement)
    {
        try
        {
            using var db = new EasyImmoContext();
            db.Evenements.Add(evenement);
            db.SaveChanges();
            return ServiceResult.Ok();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.ToString());
            return ServiceResult.Fail("Impossible de créer l'événement.");
        }
    }

    public static ServiceResult UpdateEvenement(Evenement evenement)
    {
        try
        {
            using var db = new EasyImmoContext();

            var existing = db.Evenements.FirstOrDefault(e => e.EvenementId == evenement.EvenementId);
            if (existing == null)
                return ServiceResult.Fail("Événement introuvable.");

            existing.Description = evenement.Description;
            existing.DateEvenement = evenement.DateEvenement;
            existing.HeureDebut = evenement.HeureDebut;
            existing.EstAccompli = evenement.EstAccompli;
            existing.TypeEvenementId = evenement.TypeEvenementId;

            db.SaveChanges();
            return ServiceResult.Ok();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.ToString());
            return ServiceResult.Fail("Impossible de mettre à jour l'événement.");
        }
    }

    public static ServiceResult DeleteEvenement(int id)
    {
        try
        {
            using var db = new EasyImmoContext();

            var evenement = db.Evenements.Find(id);
            if (evenement == null)
                return ServiceResult.Fail("L'événement n'existe pas.");

            db.Evenements.Remove(evenement);
            db.SaveChanges();
            return ServiceResult.Ok();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.ToString());
            return ServiceResult.Fail("Impossible de supprimer l'événement.");
        }
    }

    public static List<RoleEvenementPersonne> GetRolesEvenement()
    {
        using var db = new EasyImmoContext();
        return db.RoleEvenementPersonnes.OrderBy(r => r.Libelle).ToList();
    }

    public static ServiceResult AddEvenement(Evenement evenement, List<RelationEvenementPersonne> participants)
    {
        try
        {
            using var db = new EasyImmoContext();
            db.Evenements.Add(evenement);
            db.SaveChanges();

            foreach (var p in participants)
            {
                p.EvenementId = evenement.EvenementId;
                db.RelationEvenementPersonnes.Add(p);
            }

            if (participants.Any())
                db.SaveChanges();

            return ServiceResult.Ok();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.ToString());
            return ServiceResult.Fail("Impossible de créer l'événement.");
        }
    }

    public static ServiceResult UpdateEvenement(Evenement evenement, List<RelationEvenementPersonne> participants)
    {
        try
        {
            using var db = new EasyImmoContext();

            var existing = db.Evenements.FirstOrDefault(e => e.EvenementId == evenement.EvenementId);
            if (existing == null)
                return ServiceResult.Fail("Événement introuvable.");

            existing.Description = evenement.Description;
            existing.DateEvenement = evenement.DateEvenement;
            existing.HeureDebut = evenement.HeureDebut;
            existing.TypeEvenementId = evenement.TypeEvenementId;
            existing.BienId = evenement.BienId;

            // Remplacer les participants
            var anciensParticipants = db.RelationEvenementPersonnes
                .Where(r => r.EvenementId == evenement.EvenementId)
                .ToList();
            db.RelationEvenementPersonnes.RemoveRange(anciensParticipants);

            foreach (var p in participants)
            {
                p.EvenementId = evenement.EvenementId;
                db.RelationEvenementPersonnes.Add(p);
            }

            db.SaveChanges();
            return ServiceResult.Ok();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.ToString());
            return ServiceResult.Fail("Impossible de mettre à jour l'événement.");
        }
    }
}