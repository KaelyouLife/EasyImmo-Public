using EasyImmoMAUI.ViewModels;

namespace EasyImmoMAUI.Views;

public partial class DashboardPage : ContentPage
{
    private readonly DashboardViewModel _viewModel;

    public DashboardPage()
    {
        InitializeComponent();
        _viewModel = new DashboardViewModel();
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.LoadDashboard();
    }

    private async void OnVoirTousLesBiensTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("//BiensPage");
    }

    private async void OnVoirAgendaTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("//EvenementsPage");
    }

    private void OnVoirTousLesBiensPointerEntered(object sender, PointerEventArgs e)
    {
        VoirTousLesBiensBorder.BackgroundColor = Color.FromArgb("#EEF2FF");
        VoirTousLesBiensBorder.Scale = 1.03;
        VoirTousLesBiensLabel.TextColor = Color.FromArgb("#3248E5");
    }

    private void OnVoirTousLesBiensPointerExited(object sender, PointerEventArgs e)
    {
        VoirTousLesBiensBorder.BackgroundColor = Colors.Transparent;
        VoirTousLesBiensBorder.Scale = 1;
        VoirTousLesBiensLabel.TextColor = Color.FromArgb("#4F63FF");
    }

    private void OnVoirAgendaPointerEntered(object sender, PointerEventArgs e)
    {
        VoirAgendaBorder.BackgroundColor = Color.FromArgb("#EEF2FF");
        VoirAgendaBorder.Scale = 1.03;
        VoirAgendaLabel.TextColor = Color.FromArgb("#3248E5");
    }

    private void OnVoirAgendaPointerExited(object sender, PointerEventArgs e)
    {
        VoirAgendaBorder.BackgroundColor = Colors.Transparent;
        VoirAgendaBorder.Scale = 1;
        VoirAgendaLabel.TextColor = Color.FromArgb("#4F63FF");
    }
}