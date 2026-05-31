using Common;
using Common.Utilities;
using DAL.DB;
using Microsoft.EntityFrameworkCore;

namespace BU.Services;

public class DocumentService
{
    public static List<DocumentBien> GetDocumentsByBienId(int bienId)
    {
        using var db = new EasyImmoContext();
        return db.DocumentBiens
            .Where(d => d.BienId == bienId)
            .Include(d => d.TypeDocument)
            .OrderByDescending(d => d.DateCreation)
            .ToList();
    }

    public static List<TypeDocument> GetTypesDocument()
    {
        using var db = new EasyImmoContext();
        return db.TypeDocuments.OrderBy(t => t.Libelle).ToList();
    }

    public static ServiceResult AddDocumentBien(int bienId, string cheminSource, string description, int? typeDocumentId)
    {
        try
        {
            string dossierBien = Path.Combine(Constants.BasePath,"Documents", $"Bien_{bienId}");

            if (!Directory.Exists(dossierBien))
                Directory.CreateDirectory(dossierBien);

            string nomFichier = Path.GetFileName(cheminSource);
            string cheminDestination = Path.Combine(dossierBien, nomFichier);
            File.Copy(cheminSource, cheminDestination, overwrite: true);

            using var db = new EasyImmoContext();
            db.DocumentBiens.Add(new DocumentBien
            {
                BienId = bienId,
                Description = description.Length > 50 ? description[..50] : description,
                Chemin = cheminDestination,
                DateCreation = DateOnly.FromDateTime(DateTime.Now),
                TypeDocumentId = typeDocumentId
            });

            db.SaveChanges();
            return ServiceResult.Ok();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.ToString());
            return ServiceResult.Fail("Impossible d'ajouter le document.");
        }
    }

    public static ServiceResult DeleteDocumentBien(int documentBienId)
    {
        try
        {
            using var db = new EasyImmoContext();

            var document = db.DocumentBiens.Find(documentBienId);
            if (document == null)
                return ServiceResult.Fail("Document introuvable.");

            // Supprimer le fichier physique si existant
            if (File.Exists(document.Chemin))
                File.Delete(document.Chemin);

            db.DocumentBiens.Remove(document);
            db.SaveChanges();
            return ServiceResult.Ok();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.ToString());
            return ServiceResult.Fail("Impossible de supprimer le document.");
        }
    }
}