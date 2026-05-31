using EasyImmoMAUI.ViewModels.ReglagesViewModels;

namespace EasyImmoMAUI.Views.ReglagesView;

public partial class ReglagesPage : ContentPage
{
    private readonly ReglagesPageViewModel _viewModel;

    public ReglagesPage()
    {
        InitializeComponent();
        _viewModel = new ReglagesPageViewModel();
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.LoadReglages();
    }

    private async void OnAjouterTypeBienClicked(object sender, EventArgs e)
    {
        var result = _viewModel.AjouterTypeBien();
        if (!result.Success)
            await DisplayAlert("Erreur", result.ErrorMessage, "OK");
    }

    private async void OnSupprimerTypeBienClicked(object sender, TappedEventArgs e)
    {
        if (e.Parameter is not int id) return;
        bool confirm = await DisplayAlert("Supprimer", "Supprimer ce type de bien ?", "Supprimer", "Annuler");
        if (!confirm) return;
        var result = _viewModel.SupprimerTypeBien(id);
        if (!result.Success)
            await DisplayAlert("Erreur", result.ErrorMessage, "OK");
    }

    private async void OnAjouterTypeDocumentClicked(object sender, EventArgs e)
    {
        var result = _viewModel.AjouterTypeDocument();
        if (!result.Success)
            await DisplayAlert("Erreur", result.ErrorMessage, "OK");
    }

    private async void OnSupprimerTypeDocumentClicked(object sender, TappedEventArgs e)
    {
        if (e.Parameter is not int id) return;
        bool confirm = await DisplayAlert("Supprimer", "Supprimer ce type de document ?", "Supprimer", "Annuler");
        if (!confirm) return;
        var result = _viewModel.SupprimerTypeDocument(id);
        if (!result.Success)
            await DisplayAlert("Erreur", result.ErrorMessage, "OK");
    }

    private async void OnAjouterTypeEvenementClicked(object sender, EventArgs e)
    {
        var result = _viewModel.AjouterTypeEvenement();
        if (!result.Success)
            await DisplayAlert("Erreur", result.ErrorMessage, "OK");
    }

    private async void OnSupprimerTypeEvenementClicked(object sender, TappedEventArgs e)
    {
        if (e.Parameter is not int id) return;
        bool confirm = await DisplayAlert("Supprimer", "Supprimer ce type d'événement ?", "Supprimer", "Annuler");
        if (!confirm) return;
        var result = _viewModel.SupprimerTypeEvenement(id);
        if (!result.Success)
            await DisplayAlert("Erreur", result.ErrorMessage, "OK");
    }

    private async void OnAjouterStatutBienClicked(object sender, EventArgs e)
    {
        var result = _viewModel.AjouterStatutBien();
        if (!result.Success)
            await DisplayAlert("Erreur", result.ErrorMessage, "OK");
    }

    private async void OnSupprimerStatutBienClicked(object sender, TappedEventArgs e)
    {
        if (e.Parameter is not int id) return;
        bool confirm = await DisplayAlert("Supprimer", "Supprimer ce statut ?", "Supprimer", "Annuler");
        if (!confirm) return;
        var result = _viewModel.SupprimerStatutBien(id);
        if (!result.Success)
            await DisplayAlert("Erreur", result.ErrorMessage, "OK");
    }
}