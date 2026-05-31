using EasyImmoMAUI.ViewModels.BiensViewModels;

namespace EasyImmoMAUI.Views;

public partial class AjouterBienPage : ContentPage
{
    private readonly AjouterBienViewModel _viewModel;

    public AjouterBienPage()
    {
        InitializeComponent();
        _viewModel = new AjouterBienViewModel();
        BindingContext = _viewModel;
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        var result = _viewModel.Save();

        if (result.Success)
        {
            await DisplayAlert("Succès", "Le bien a été ajouté.", "OK");
            await Navigation.PopAsync();
        }
        else
        {
            await DisplayAlert("Erreur", result.ErrorMessage, "OK");
        }
    }
}