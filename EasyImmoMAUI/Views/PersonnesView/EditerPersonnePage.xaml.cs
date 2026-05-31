using EasyImmoMAUI.ViewModels.PersonnesViewModels;

namespace EasyImmoMAUI.Views.PersonnesView;

[QueryProperty(nameof(PersonneId), "personneId")]
public partial class EditerPersonnePage : ContentPage
{
    private readonly EditerPersonneViewModel _viewModel;

    public string PersonneId
    {
        set
        {
            if (int.TryParse(value, out int id))
                _viewModel.ChargerPersonne(id);
        }
    }

    public EditerPersonnePage()
    {
        InitializeComponent();
        _viewModel = new EditerPersonneViewModel();
        BindingContext = _viewModel;
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        var result = _viewModel.Save();
        if (result.Success)
        {
            await DisplayAlert("Succès", "Personne modifiée.", "OK");
            await Navigation.PopAsync();
        }
        else
        {
            await DisplayAlert("Erreur", result.ErrorMessage, "OK");
        }
    }
}