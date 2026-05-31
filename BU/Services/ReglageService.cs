using Common.Models;
using Common.Utilities;
using DAL.DB;

namespace BU.Services;

public static class ReglageService
{

    // GetAll, Delete, Create pour les types de biens
    public static List<ReglageItemModel> GetTypesBien()
    {
        using var db = new EasyImmoContext();

        return db.TypeBiens
            .Select(x => new ReglageItemModel
            {
                Id = x.TypeBienId,
                Libelle = x.Libelle
            })
            .ToList();
    }

    public static ServiceResult CreateTypeBien(TypeBien typeBien)
    {
        try
        {
            using var db = new EasyImmoContext();

            db.TypeBiens.Add(typeBien);
            db.SaveChanges();

            return ServiceResult.Ok();
        }
        catch
        {
            return ServiceResult.Fail("Impossible de créer le type de bien.");
        }
    }

    public static ServiceResult DeleteTypeBien(int id)
    {
        try
        {
            using var db = new EasyImmoContext();

            var item = db.TypeBiens.Find(id);

            if (item == null)
                return ServiceResult.Fail("Impossible de trouver le type de bien.");

            db.TypeBiens.Remove(item);
            db.SaveChanges();

            return ServiceResult.Ok();
        }
        catch
        {
            return ServiceResult.Fail("Impossible de supprimer le type de bien.");
        }
    }

    // GetAll, Delete, Create pour les types de documents
    public static List<ReglageItemModel> GetTypesDocument()
    {
        using var db = new EasyImmoContext();

        return db.TypeDocuments
            .Select(x => new ReglageItemModel
            {
                Id = x.TypeDocumentId,
                Libelle = x.Libelle
            })
            .ToList();
    }
    public static ServiceResult CreateTypeDocument(TypeDocument typeDocument)
    {
        try
        {
            using var db = new EasyImmoContext();
            db.TypeDocuments.Add(typeDocument);
            db.SaveChanges();
            return ServiceResult.Ok();
        }
        catch
        {
            return ServiceResult.Fail("Impossible de créer le type de document.");
        }
    }

    public static ServiceResult DeleteTypeDocument(int id)
    {
        try
        {
            using var db = new EasyImmoContext();
            var item = db.TypeDocuments.Find(id);
            if (item == null)
                return ServiceResult.Fail("Impossible de trouver le type de document.");
            db.TypeDocuments.Remove(item);
            db.SaveChanges();
            return ServiceResult.Ok();
        }
        catch
        {
            return ServiceResult.Fail("Impossible de supprimer le type de document.");
        }
    }

    // GetAll, Delete, Create pour les types d'évènements

    public static List<ReglageItemModel> GetTypesEvenement()
    {
        using var db = new EasyImmoContext();

        return db.TypeEvenements
            .Select(x => new ReglageItemModel
            {
                Id = x.TypeEvenementId,
                Libelle = x.Libelle
            })
            .ToList();
    }

    public static ServiceResult CreateTypeEvenement(TypeEvenement typeEvenement)
    {
        try
        {
            using var db = new EasyImmoContext();
            db.TypeEvenements.Add(typeEvenement);
            db.SaveChanges();
            return ServiceResult.Ok();
        }
        catch
        {
            return ServiceResult.Fail("Impossible de créer le type d'évènement.");
        }
    }

    public static ServiceResult DeleteTypeEvenement(int id)
    {
        try
        {
            using var db = new EasyImmoContext();
            var item = db.TypeEvenements.Find(id);
            if (item == null)
                return ServiceResult.Fail("Impossible de trouver le type d'évènement.");
            db.TypeEvenements.Remove(item);
            db.SaveChanges();
            return ServiceResult.Ok();
        }
        catch
        {
            return ServiceResult.Fail("Impossible de supprimer le type d'évènement.");
        }
    }

    // GetAll, Delete, Create pour les types de statuts de biens

    public static List<ReglageItemModel> GetStatutsBien()
    {
        using var db = new EasyImmoContext();

        return db.StatutBiens
            .Select(x => new ReglageItemModel
            {
                Id = x.StatutBienId,
                Libelle = x.Libelle
            })
            .ToList();
    }

    public static ServiceResult CreateStatutBien(StatutBien statutBien)
    {
        try
        {
            using var db = new EasyImmoContext();
            db.StatutBiens.Add(statutBien);
            db.SaveChanges();
            return ServiceResult.Ok();
        }
        catch
        {
            return ServiceResult.Fail("Impossible de créer le statut de bien.");
        }
    }

    public static ServiceResult DeleteStatutBien(int id)
    {
        try
        {
            using var db = new EasyImmoContext();
            var item = db.StatutBiens.Find(id);
            if (item == null)
                return ServiceResult.Fail("Impossible de trouver le statut de bien.");
            db.StatutBiens.Remove(item);
            db.SaveChanges();
            return ServiceResult.Ok();
        }
        catch
        {
            return ServiceResult.Fail("Impossible de supprimer le statut de bien.");
        }
    }
}