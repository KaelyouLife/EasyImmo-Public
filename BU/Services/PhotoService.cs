using Common.Utilities;
using Common;
using DAL.DB;

namespace BU.Services;

public class PhotoService
{
    public static List<PhotoBien> GetPhotosByBienId(int bienId)
    {
        using var db = new EasyImmoContext();
        return db.PhotoBiens
            .Where(p => p.BienId == bienId)
            .OrderByDescending(p => p.EstPrincipale)
            .ToList();
    }

    /// <summary>
    /// Copie le fichier image dans LocalAppData/Photos/Bien_X/ et enregistre la photo
    /// Si c'est la première photo du bien, elle devient automatiquement principale
    /// </summary>
    public static ServiceResult AddPhoto(int bienId, string cheminSource)
    {
        try
        {
            string dossier = Path.Combine(
                Path.Combine(Constants.BasePath), "Photos", $"Bien_{bienId}");

            if (!Directory.Exists(dossier))
                Directory.CreateDirectory(dossier);

            string nomFichier = Path.GetFileName(cheminSource);
            string destination = Path.Combine(dossier, nomFichier);
            File.Copy(cheminSource, destination, overwrite: true);

            using var db = new EasyImmoContext();

            bool estPremiere = !db.PhotoBiens.Any(p => p.BienId == bienId);

            db.PhotoBiens.Add(new PhotoBien
            {
                BienId = bienId,
                Chemin = destination,
                EstPrincipale = estPremiere
            });

            db.SaveChanges();
            return ServiceResult.Ok();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.ToString());
            return ServiceResult.Fail("Impossible d'ajouter la photo.");
        }
    }

    public static ServiceResult DefinirComoPrincipale(int photoBienId)
    {
        try
        {
            using var db = new EasyImmoContext();

            var photo = db.PhotoBiens.Find(photoBienId);
            if (photo == null)
                return ServiceResult.Fail("Photo introuvable.");

            var photos = db.PhotoBiens.Where(p => p.BienId == photo.BienId).ToList();
            foreach (var p in photos)
                p.EstPrincipale = false;

            photo.EstPrincipale = true;
            db.SaveChanges();
            return ServiceResult.Ok();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.ToString());
            return ServiceResult.Fail("Impossible de définir la photo principale.");
        }
    }

    /// <summary>
    /// Supprime une photo et son fichier physique
    /// Si c'est la principale, la première photo devient principale
    /// </summary>
    public static ServiceResult DeletePhoto(int photoBienId)
    {
        try
        {
            using var db = new EasyImmoContext();

            var photo = db.PhotoBiens.Find(photoBienId);
            if (photo == null)
                return ServiceResult.Fail("Photo introuvable.");

            bool etaitPrincipale = photo.EstPrincipale == true;
            int bienId = photo.BienId;

            if (File.Exists(photo.Chemin))
                File.Delete(photo.Chemin);

            db.PhotoBiens.Remove(photo);
            db.SaveChanges();

            if (etaitPrincipale)
            {
                var nouvelle = db.PhotoBiens.FirstOrDefault(p => p.BienId == bienId);
                if (nouvelle != null)
                {
                    nouvelle.EstPrincipale = true;
                    db.SaveChanges();
                }
            }

            return ServiceResult.Ok();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.ToString());
            return ServiceResult.Fail("Impossible de supprimer la photo.");
        }
    }

    public static ServiceResult DeleteAllPhotos(int bienId)
    {
        try
        {
            using var db = new EasyImmoContext();

            var photos = db.PhotoBiens.Where(p => p.BienId == bienId).ToList();

            foreach (var photo in photos)
            {
                if (File.Exists(photo.Chemin))
                    File.Delete(photo.Chemin);
            }

            db.PhotoBiens.RemoveRange(photos);
            db.SaveChanges();
            return ServiceResult.Ok();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.ToString());
            return ServiceResult.Fail("Impossible de supprimer les photos.");
        }
    }
}