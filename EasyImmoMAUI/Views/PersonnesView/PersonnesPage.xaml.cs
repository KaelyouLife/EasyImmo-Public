using EasyImmoMAUI.ViewModels.PersonnesViewModels;

namespace EasyImmoMAUI.Views.PersonnesView;

public partial class PersonnesPage : ContentPage
{
    private readonly PersonnesPageViewModel _viewModel;

    public PersonnesPage()
    {
        InitializeComponent();
        _viewModel = new PersonnesPageViewModel();
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.LoadPersonnes();
    }

    private async void OnAjouterClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("AjouterPersonnePage");
    }

    private async void OnEditerClicked(object sender, EventArgs e)
    {
        if (_viewModel.PersonneSelectionnee == null) return;
        await Shell.Current.GoToAsync($"EditerPersonnePage?personneId={_viewModel.PersonneSelectionnee.PersonneId}");
    }

    private async void OnSupprimerClicked(object sender, EventArgs e)
    {
        if (_viewModel.PersonneSelectionnee == null) return;

        bool confirm = await DisplayAlert(
            "Supprimer",
            $"Supprimer {_viewModel.PersonneSelectionnee.NomComplet} ?",
            "Supprimer", "Annuler");

        if (!confirm) return;

        var result = _viewModel.SupprimerPersonne();
        if (!result.Success)
            await DisplayAlert("Erreur", result.ErrorMessage, "OK");
    }

    private void OnToggleAjouterBienClicked(object sender, TappedEventArgs e)
    {
        PanneauAjouterBien.IsVisible = !PanneauAjouterBien.IsVisible;
    }

    private async void OnAjouterBienLieClicked(object sender, EventArgs e)
    {
        var result = _viewModel.AjouterBienLie();
        if (!result.Success)
            await DisplayAlert("Erreur", result.ErrorMessage, "OK");
        else
            PanneauAjouterBien.IsVisible = false;
    }

    private async void OnSupprimerBienLieClicked(object sender, TappedEventArgs e)
    {
        if (e.Parameter is not int relationId) return;

        bool confirm = await DisplayAlert("Supprimer", "Retirer ce bien ?", "Supprimer", "Annuler");
        if (!confirm) return;

        var result = _viewModel.SupprimerBienLie(relationId);
        if (!result.Success)
            await DisplayAlert("Erreur", result.ErrorMessage, "OK");
    }
}