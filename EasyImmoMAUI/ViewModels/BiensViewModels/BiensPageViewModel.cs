using BU.Services;
using Common.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace EasyImmoMAUI.ViewModels.BiensViewModels;

public class BiensPageViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    private List<BienResumeModel> _tousLesBiens = new();
    private ObservableCollection<BienResumeModel> _biens = new();
    private string _recherche = string.Empty;

    public ObservableCollection<BienResumeModel> Biens
    {
        get => _biens;
        set { _biens = value; OnPropertyChanged(nameof(Biens)); }
    }

    public string Recherche
    {
        get => _recherche;
        set
        {
            _recherche = value;
            OnPropertyChanged(nameof(Recherche));
            AppliquerFiltres();
        }
    }

    public ICommand OuvrirDetailBienCommand { get; }

    public BiensPageViewModel()
    {
        OuvrirDetailBienCommand = new Command<int>(async bienId =>
        {
            await Shell.Current.GoToAsync($"DetailBienPage?bienId={bienId}");
        });
    }
    public void LoadBiens()
    {
        _tousLesBiens = BienService.GetBiensResume();
        AppliquerFiltres();
    }

    private void AppliquerFiltres()
    {
        var filtres = _tousLesBiens.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(Recherche))
            filtres = filtres.Where(b =>
                b.TitreAnnonce != null &&
                b.TitreAnnonce.Contains(Recherche, StringComparison.OrdinalIgnoreCase));

        Biens = new ObservableCollection<BienResumeModel>(filtres);
    }
}