using EasyImmoMAUI.ViewModels;

namespace EasyImmoMAUI.Views.EvenementsView;

public partial class EvenementsPage : ContentPage
{
    private readonly EvenementsViewModel _viewModel;

    public EvenementsPage()
    {
        InitializeComponent();
        _viewModel = new EvenementsViewModel();
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.Initialiser();
    }

    private void OnAjouterClicked(object sender, EventArgs e)
    {
        _viewModel.OuvrirFormulaireAjout();
    }

    private void OnEditerClicked(object sender, EventArgs e)
    {
        _viewModel.OuvrirFormulaireEdition();
    }

    private async void OnSupprimerClicked(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert(
            "Supprimer",
            "Supprimer cet événement ?",
            "Supprimer", "Annuler");

        if (!confirm) return;

        var result = _viewModel.Supprimer();
        if (!result.Success)
            await DisplayAlert("Erreur", result.ErrorMessage, "OK");
    }

    private async void OnEnregistrerClicked(object sender, EventArgs e)
    {
        var result = _viewModel.Enregistrer();
        if (!result.Success)
            await DisplayAlert("Erreur", result.ErrorMessage, "OK");
    }

    private void OnAnnulerClicked(object sender, EventArgs e)
    {
        _viewModel.FormulaireVisible = false;
    }

    private void OnAjouterParticipantClicked(object sender, EventArgs e)
    {
        _viewModel.AjouterParticipant();
    }

    private void OnRetirerParticipantClicked(object sender, TappedEventArgs e)
    {
        if (e.Parameter is not EvenementsViewModel.ParticipantRow participant) return;
        _viewModel.RetirerParticipant(participant);
    }
}