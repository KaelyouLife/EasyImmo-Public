using BU.Services;
using Common.Utilities;
using DAL.DB;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EasyImmoMAUI.ViewModels.BiensViewModels;

public class DocumentsBienViewModel : INotifyPropertyChanged
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
    private TypeDocument? _typeAjoutSelectionne;
    private TypeDocument? _typeFiltreSelectionne;
    private ObservableCollection<DocumentBien> _documents = new();
    private ObservableCollection<DocumentBien> _documentsFiltres = new();

    public ObservableCollection<DocumentBien> DocumentsFiltres
    {
        get => _documentsFiltres;
        private set => SetField(ref _documentsFiltres, value);
    }

    public List<TypeDocument> TypesDocument { get; private set; } = new();

    public List<TypeDocument?> TypesDocumentFiltre { get; private set; } = new();

    public TypeDocument? TypeAjoutSelectionne
    {
        get => _typeAjoutSelectionne;
        set => SetField(ref _typeAjoutSelectionne, value);
    }

    public TypeDocument? TypeFiltreSelectionne
    {
        get => _typeFiltreSelectionne;
        set
        {
            SetField(ref _typeFiltreSelectionne, value);
            AppliquerFiltres();
        }
    }

    public void Initialiser(int bienId)
    {
        _bienId = bienId;
        TypesDocument = DocumentService.GetTypesDocument();
        TypesDocumentFiltre = new List<TypeDocument?> { null }.Concat(TypesDocument).ToList();
        OnPropertyChanged(nameof(TypesDocument));
        OnPropertyChanged(nameof(TypesDocumentFiltre));
        ChargerDocuments();
    }

    public void ChargerDocuments()
    {
        var liste = DocumentService.GetDocumentsByBienId(_bienId);
        _documents = new ObservableCollection<DocumentBien>(liste);
        AppliquerFiltres();
    }

    private void AppliquerFiltres()
    {
        var filtres = _documents.AsEnumerable();

        if (TypeFiltreSelectionne != null)
            filtres = filtres.Where(d => d.TypeDocumentId == TypeFiltreSelectionne.TypeDocumentId);

        DocumentsFiltres = new ObservableCollection<DocumentBien>(filtres);
    }

    public async Task<ServiceResult> AjouterDocument()
    {
        if (TypeAjoutSelectionne == null)
            return ServiceResult.Fail("Veuillez sélectionner un type de document avant d'ajouter.");

        var options = new PickOptions
        {
            PickerTitle = "Sélectionner un document",
            FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                { DevicePlatform.WinUI, new[] { ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".jpg", ".png" } },
            })
        };

        var fichier = await FilePicker.PickAsync(options);
        if (fichier == null)
            return ServiceResult.Ok(); // annulé par l'utilisateur

        var result = DocumentService.AddDocumentBien(
            bienId: _bienId,
            cheminSource: fichier.FullPath,
            description: Path.GetFileNameWithoutExtension(fichier.FileName),
            typeDocumentId: TypeAjoutSelectionne.TypeDocumentId
        );

        if (result.Success)
        {
            TypeAjoutSelectionne = null;
            ChargerDocuments();
        }

        return result;
    }

    public ServiceResult SupprimerDocument(int documentBienId)
    {
        var result = DocumentService.DeleteDocumentBien(documentBienId);
        if (result.Success)
            ChargerDocuments();
        return result;
    }
}