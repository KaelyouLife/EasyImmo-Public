using EasyImmoMAUI.ViewModels.PersonnesViewModels;

namespace EasyImmoMAUI.Views.PersonnesView;

public partial class AjouterPersonnePage : ContentPage
{
    private readonly AjouterPersonneViewModel _viewModel;

    public AjouterPersonnePage()
    {
        InitializeComponent();
        _viewModel = new AjouterPersonneViewModel();
        BindingContext = _viewModel;
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        var result = _viewModel.Save();
        if (result.Success)
        {
            await DisplayAlert("Succès", "Personne ajoutée.", "OK");
            await Navigation.PopAsync();
        }
        else
        {
            await DisplayAlert("Erreur", result.ErrorMessage, "OK");
        }
    }
}