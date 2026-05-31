using EasyImmoMAUI.ViewModels.BiensViewModels;

namespace EasyImmoMAUI.Views;

[QueryProperty(nameof(BienId), "bienId")]
public partial class EditerBienPage : ContentPage
{
    private readonly EditerBienViewModel _viewModel;

    public string BienId
    {
        set
        {
            if (int.TryParse(value, out int id))
                _viewModel.ChargerBien(id);
        }
    }

    public EditerBienPage()
    {
        InitializeComponent();
        _viewModel = new EditerBienViewModel();
        BindingContext = _viewModel;
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        var result = _viewModel.Save();

        if (result.Success)
        {
            await DisplayAlert("Succès", "Le bien a été modifié.", "OK");
            await Navigation.PopAsync();
        }
        else
        {
            await DisplayAlert("Erreur", result.ErrorMessage, "OK");
        }
    }
}