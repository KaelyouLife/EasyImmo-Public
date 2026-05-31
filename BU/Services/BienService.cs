using Common.Models;
using Common.Utilities;
using Common;
using DAL.DB;
using Microsoft.EntityFrameworkCore;

namespace BU.Services;

public class BienService
{
    public static List<DAL.DB.Bien> GetBiens()
    {
        using var db = new DAL.DB.EasyImmoContext();
        return db.Biens.ToList();
    }

    public ServiceResult AddBien(Bien bien)
    {
        try
        {
            using var db = new EasyImmoContext();

            db.Adresses.Add(bien.Adresse);
            db.Biens.Add(bien);

            db.SaveChanges();
            CreateFoldersForBien(bien);
            return ServiceResult.Ok();
        }
        catch
        {
            return ServiceResult.Fail("Une erreur est survenue lors de l'ajout du bien.");
        }
    }

    public static ServiceResult CreateFoldersForBien(Bien bien)
    {
        try
        {
            string photosPath = Path.Combine(Constants.BasePath, "Photos", $"Bien_{bien.BienId}");
            string documentsPath = Path.Combine(Constants.BasePath, "Documents", $"Bien_{bien.BienId}");

            if (!Directory.Exists(photosPath))
                Directory.CreateDirectory(photosPath);

            if (!Directory.Exists(documentsPath))
                Directory.CreateDirectory(documentsPath);

            return ServiceResult.Ok();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.ToString());
            return ServiceResult.Fail("Impossible de créer les dossiers pour le bien.");
        }
    }

    public static List<TypeBien> GetTypesBien()
    {
        using var db = new EasyImmoContext();
        return db.TypeBiens.OrderBy(t => t.Libelle).ToList();
    }

    public static List<StatutBien> GetStatutsBien()
    {
        using var db = new EasyImmoContext();
        return db.StatutBiens.OrderBy(s => s.StatutBienId).ToList();
    }

    public static Bien GetBienById(int id)
    {
        using var db = new EasyImmoContext();
        return db.Biens
            .Include(b=>b.RelationBienPersonnes)
                .ThenInclude(rbp => rbp.Personne)
            .Include(b => b.HistoriqueStatutBiens)
            .Include (b => b.Evenements)
            .Include(b => b.DocumentBiens)
            .Include(b => b.Adresse)
            .Include(b => b.TypeBien)
            .Include(b => b.StatutBien)
            .Include(b => b.PhotoBiens)
            .FirstOrDefault(b => b.BienId == id);
    }

    public ServiceResult UpdateBien(DAL.DB.Bien bien)
    {
        try
        {
            using var db = new DAL.DB.EasyImmoContext();

            var existingBien = db.Biens.FirstOrDefault(b => b.BienId == bien.BienId);
            if (existingBien == null)
                return ServiceResult.Fail("Bien introuvable.");

            var existingAdresse = db.Adresses.FirstOrDefault(a => a.AdresseId == existingBien.AdresseId);
            if (existingAdresse == null)
                return ServiceResult.Fail("Adresse introuvable.");

            // Sauvegarder les valeurs à préserver
            var createdAt = existingBien.CreatedAt;
            var adresseId = existingBien.AdresseId; // <-- ajout

            db.Entry(existingBien).CurrentValues.SetValues(bien);

            // Restaurer
            existingBien.CreatedAt = createdAt;
            existingBien.AdresseId = adresseId;  // <-- ajout
            existingBien.UpdatedAt = DateTime.Now;

            // Mettre à jour l'adresse séparément
            existingAdresse.Rue = bien.Adresse.Rue;
            existingAdresse.Numero = bien.Adresse.Numero;
            existingAdresse.Boite = bien.Adresse.Boite;
            existingAdresse.CodePostal = bien.Adresse.CodePostal;
            existingAdresse.Commune = bien.Adresse.Commune;

            db.SaveChanges();
            return ServiceResult.Ok();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.ToString());
            return ServiceResult.Fail("Impossible d'éditer le bien.");
        }
    }

    public ServiceResult DeleteBien(int id)
    {
        try
        {
            using var db = new DAL.DB.EasyImmoContext();

            var bien = db.Biens.Find(id);

            if (bien == null)
                return ServiceResult.Fail("Impossible de supprimer le bien. Bien introuvable.");

            if (db.Transactions.Any(t => t.BienId == id))
                return ServiceResult.Fail("Ce bien ne peut pas être supprimé car il est lié à des transactions.");

            db.Biens.Remove(bien);
            db.SaveChanges();

            return ServiceResult.Ok();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.ToString());
            return ServiceResult.Fail("Impossible de supprimer le bien.");
        }
    }

    public static List<Common.Models.BienResumeModel> GetDerniersBiensAjoutes(int nombre)
    {
        using var db = new DAL.DB.EasyImmoContext();

        return db.Biens
            .OrderByDescending(b => b.CreatedAt)
            .Take(nombre)
            .Select(b => new BienResumeModel
            {
                BienId = b.BienId,
                TitreAnnonce = b.TitreAnnonce,
                Commune = b.Adresse.Commune,
                Statut = b.StatutBien.Libelle,
                PhotoPrincipale = b.PhotoBiens
                    .Where(p => p.EstPrincipale == true)
                    .Select(p => p.Chemin)
                    .FirstOrDefault()
            })
            .ToList();
    }

    public static int GetNombreBiensDisponibles()
    {
        using var db = new EasyImmoContext();

        return db.Biens.Count(b => 
        b.StatutBienId == 1 ||
        b.StatutBienId == 3);
    }

    public static List<BienResumeModel> GetBiensResume()
    {
        using var db = new EasyImmoContext();

        return db.Biens
            .Include(b => b.TypeBien)
            .Include(b => b.Adresse)
            .Include(b => b.StatutBien)
            .Include(b => b.PhotoBiens)
            .Select(b => new BienResumeModel
            {
                BienId = b.BienId,
                TitreAnnonce = b.TitreAnnonce,
                TypeBien = b.TypeBien.Libelle,
                CodePostal = b.Adresse.CodePostal,
                Commune = b.Adresse.Commune,
                Statut = b.StatutBien.Libelle,
                PhotoPrincipale = b.PhotoBiens
                    .Where(p => p.EstPrincipale == true)
                    .Select(p => p.Chemin)
                    .FirstOrDefault()
            })
            .ToList();
    }

    // Retourne l'historique des statuts d'un bien
    public static List<HistoriqueStatutBien> GetHistoriqueByBienId(int bienId)
    {
        using var db = new EasyImmoContext();
        return db.HistoriqueStatutBiens
            .Where(h => h.BienId == bienId)
            .Include(h => h.StatutBien)
            .OrderByDescending(h => h.DateChangement)
            .ToList();
    }
}