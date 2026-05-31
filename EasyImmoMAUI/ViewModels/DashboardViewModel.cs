using BU.Services;
using Common.Models;
using DAL.DB;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EasyImmoMAUI.ViewModels;

public class DashboardViewModel : INotifyPropertyChanged
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

    private ObservableCollection<BienResumeModel> _derniersBiens = new();
    public ObservableCollection<BienResumeModel> DerniersBiens
    {
        get => _derniersBiens;
        private set => SetField(ref _derniersBiens, value);
    }

    private ObservableCollection<Evenement> _prochainsEvenements = new();
    public ObservableCollection<Evenement> ProchainsEvenements
    {
        get => _prochainsEvenements;
        private set => SetField(ref _prochainsEvenements, value);
    }

    private int _nombreBiensDisponibles;
    public int NombreBiensDisponibles
    {
        get => _nombreBiensDisponibles;
        private set => SetField(ref _nombreBiensDisponibles, value);
    }

    private int _nombreEvenementsAvenir;
    public int NombreEvenementsAvenir
    {
        get => _nombreEvenementsAvenir;
        private set => SetField(ref _nombreEvenementsAvenir, value);
    }

    public void LoadDashboard()
    {
        DerniersBiens = new ObservableCollection<BienResumeModel>(
            BienService.GetDerniersBiensAjoutes(5));

        NombreBiensDisponibles = BienService.GetNombreBiensDisponibles();

        var evenements = EvenementService.GetEvenementsByWeek();
        ProchainsEvenements = new ObservableCollection<Evenement>(evenements.Take(5));
        NombreEvenementsAvenir = evenements.Count;
    }
}