using BU.Services;
using Common.Utilities;
using DAL.DB;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EasyImmoMAUI.ViewModels.BiensViewModels;

public class PhotosBienViewModel : INotifyPropertyChanged
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
    private ObservableCollection<PhotoBien> _photos = new();
    private int _nombrePhotos;
    private PhotoBien? _photoPrincipale;

    public ObservableCollection<PhotoBien> Photos
    {
        get => _photos;
        private set => SetField(ref _photos, value);
    }

    public int NombrePhotos
    {
        get => _nombrePhotos;
        private set => SetField(ref _nombrePhotos, value);
    }

    public PhotoBien? PhotoPrincipale
    {
        get => _photoPrincipale;
        private set => SetField(ref _photoPrincipale, value);
    }

    public bool ADesPhotos => NombrePhotos > 0;

    public void Initialiser(int bienId)
    {
        _bienId = bienId;
        ChargerPhotos();
    }

    public void ChargerPhotos()
    {
        var liste = PhotoService.GetPhotosByBienId(_bienId);
        Photos = new ObservableCollection<PhotoBien>(liste);
        NombrePhotos = liste.Count;
        PhotoPrincipale = liste.FirstOrDefault(p => p.EstPrincipale == true);
        OnPropertyChanged(nameof(ADesPhotos));
    }

    public async Task<ServiceResult> AjouterPhotos()
    {
        var options = new PickOptions
        {
            PickerTitle = "Sélectionner des photos",
            FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                { DevicePlatform.WinUI, new[] { ".jpg", ".jpeg", ".png", ".webp" } },
            })
        };

        var fichiers = await FilePicker.PickMultipleAsync(options);
        if (fichiers == null || !fichiers.Any())
            return ServiceResult.Ok(); // annulé

        ServiceResult dernierResultat = ServiceResult.Ok();
        foreach (var fichier in fichiers)
        {
            var result = PhotoService.AddPhoto(_bienId, fichier.FullPath);
            if (!result.Success)
                dernierResultat = result;
        }

        ChargerPhotos();
        return dernierResultat;
    }

    public ServiceResult DefinirCommePrincipale(int photoBienId)
    {
        var result = PhotoService.DefinirComoPrincipale(photoBienId);
        if (result.Success)
            ChargerPhotos();
        return result;
    }

    public ServiceResult SupprimerPhoto(int photoBienId)
    {
        var result = PhotoService.DeletePhoto(photoBienId);
        if (result.Success)
            ChargerPhotos();
        return result;
    }

    public ServiceResult SupprimerToutesLesPhotos()
    {
        var result = PhotoService.DeleteAllPhotos(_bienId);
        if (result.Success)
            ChargerPhotos();
        return result;
    }
}