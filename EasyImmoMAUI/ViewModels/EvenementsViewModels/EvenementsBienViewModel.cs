using BU.Services;
using DAL.DB;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EasyImmoMAUI.ViewModels.BiensViewModels;

public class EvenementsBienViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    private ObservableCollection<EvenementRow> _evenements = new();
    public ObservableCollection<EvenementRow> Evenements
    {
        get => _evenements;
        private set
        {
            _evenements = value;
            OnPropertyChanged();
        }
    }

    public void Initialiser(int bienId)
    {
        var liste = EvenementService.GetEvenementsByBienId(bienId);

        Evenements = new ObservableCollection<EvenementRow>(
            liste.Select(e => new EvenementRow
            {
                Date = e.DateEvenement.ToString("dd/MM/yyyy"),
                Heure = e.HeureDebut.ToString("HH:mm"),
                Description = e.Description ?? "—",
                TypeLibelle = e.TypeEvenement?.Libelle ?? "—",
                Participants = e.RelationEvenementPersonnes
                    .Where(r => r.Personne != null)
                    .Select(r => $"{r.Personne.Prenom} {r.Personne.Nom}")
                    .ToList()
            })
        );
    }

    public class EvenementRow
    {
        public string Date { get; set; } = string.Empty;
        public string Heure { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string TypeLibelle { get; set; } = string.Empty;
        public List<string> Participants { get; set; } = new();

        // Participants formatés en une seule ligne pour l'affichage
        public string ParticipantsFormates =>
            Participants.Count > 0 ? string.Join(", ", Participants) : "—";
    }
}