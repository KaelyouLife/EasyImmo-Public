using BU.Services;
using Common.Models;
using Common.Utilities;
using DAL.DB;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EasyImmoMAUI.ViewModels.ReglagesViewModels;

public class ReglagesPageViewModel : INotifyPropertyChanged
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

    private ObservableCollection<ReglageItemModel> _typesBien = new();
    public ObservableCollection<ReglageItemModel> TypesBien
    {
        get => _typesBien;
        private set => SetField(ref _typesBien, value);
    }

    private string _nouveauTypeBien = string.Empty;
    public string NouveauTypeBien
    {
        get => _nouveauTypeBien;
        set => SetField(ref _nouveauTypeBien, value);
    }


    private ObservableCollection<ReglageItemModel> _typesDocument = new();
    public ObservableCollection<ReglageItemModel> TypesDocument
    {
        get => _typesDocument;
        private set => SetField(ref _typesDocument, value);
    }

    private string _nouveauTypeDocument = string.Empty;
    public string NouveauTypeDocument
    {
        get => _nouveauTypeDocument;
        set => SetField(ref _nouveauTypeDocument, value);
    }

    private ObservableCollection<ReglageItemModel> _typesEvenement = new();
    public ObservableCollection<ReglageItemModel> TypesEvenement
    {
        get => _typesEvenement;
        private set => SetField(ref _typesEvenement, value);
    }

    private string _nouveauTypeEvenement = string.Empty;
    public string NouveauTypeEvenement
    {
        get => _nouveauTypeEvenement;
        set => SetField(ref _nouveauTypeEvenement, value);
    }

    private ObservableCollection<ReglageItemModel> _statutsBien = new();
    public ObservableCollection<ReglageItemModel> StatutsBien
    {
        get => _statutsBien;
        private set => SetField(ref _statutsBien, value);
    }

    private string _nouveauStatutBien = string.Empty;
    public string NouveauStatutBien
    {
        get => _nouveauStatutBien;
        set => SetField(ref _nouveauStatutBien, value);
    }

    public void LoadReglages()
    {
        TypesBien = new ObservableCollection<ReglageItemModel>(ReglageService.GetTypesBien());
        TypesDocument = new ObservableCollection<ReglageItemModel>(ReglageService.GetTypesDocument());
        TypesEvenement = new ObservableCollection<ReglageItemModel>(ReglageService.GetTypesEvenement());
        StatutsBien = new ObservableCollection<ReglageItemModel>(ReglageService.GetStatutsBien());
    }

    public ServiceResult AjouterTypeBien()
    {
        if (string.IsNullOrWhiteSpace(NouveauTypeBien))
            return ServiceResult.Fail("Le libellé est obligatoire.");

        var result = ReglageService.CreateTypeBien(new TypeBien { Libelle = NouveauTypeBien.Trim() });
        if (result.Success)
        {
            NouveauTypeBien = string.Empty;
            TypesBien = new ObservableCollection<ReglageItemModel>(ReglageService.GetTypesBien());
        }
        return result;
    }

    public ServiceResult SupprimerTypeBien(int id)
    {
        var result = ReglageService.DeleteTypeBien(id);
        if (result.Success)
            TypesBien = new ObservableCollection<ReglageItemModel>(ReglageService.GetTypesBien());
        return result;
    }

    public ServiceResult AjouterTypeDocument()
    {
        if (string.IsNullOrWhiteSpace(NouveauTypeDocument))
            return ServiceResult.Fail("Le libellé est obligatoire.");

        var result = ReglageService.CreateTypeDocument(new TypeDocument { Libelle = NouveauTypeDocument.Trim() });
        if (result.Success)
        {
            NouveauTypeDocument = string.Empty;
            TypesDocument = new ObservableCollection<ReglageItemModel>(ReglageService.GetTypesDocument());
        }
        return result;
    }

    public ServiceResult SupprimerTypeDocument(int id)
    {
        var result = ReglageService.DeleteTypeDocument(id);
        if (result.Success)
            TypesDocument = new ObservableCollection<ReglageItemModel>(ReglageService.GetTypesDocument());
        return result;
    }

    public ServiceResult AjouterTypeEvenement()
    {
        if (string.IsNullOrWhiteSpace(NouveauTypeEvenement))
            return ServiceResult.Fail("Le libellé est obligatoire.");

        var result = ReglageService.CreateTypeEvenement(new TypeEvenement { Libelle = NouveauTypeEvenement.Trim() });
        if (result.Success)
        {
            NouveauTypeEvenement = string.Empty;
            TypesEvenement = new ObservableCollection<ReglageItemModel>(ReglageService.GetTypesEvenement());
        }
        return result;
    }

    public ServiceResult SupprimerTypeEvenement(int id)
    {
        var result = ReglageService.DeleteTypeEvenement(id);
        if (result.Success)
            TypesEvenement = new ObservableCollection<ReglageItemModel>(ReglageService.GetTypesEvenement());
        return result;
    }

    public ServiceResult AjouterStatutBien()
    {
        if (string.IsNullOrWhiteSpace(NouveauStatutBien))
            return ServiceResult.Fail("Le libellé est obligatoire.");

        var result = ReglageService.CreateStatutBien(new StatutBien { Libelle = NouveauStatutBien.Trim() });
        if (result.Success)
        {
            NouveauStatutBien = string.Empty;
            StatutsBien = new ObservableCollection<ReglageItemModel>(ReglageService.GetStatutsBien());
        }
        return result;
    }

    public ServiceResult SupprimerStatutBien(int id)
    {
        var result = ReglageService.DeleteStatutBien(id);
        if (result.Success)
            StatutsBien = new ObservableCollection<ReglageItemModel>(ReglageService.GetStatutsBien());
        return result;
    }
}