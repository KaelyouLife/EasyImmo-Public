using EasyImmoMAUI.ViewModels.BiensViewModels;

namespace EasyImmoMAUI.Views.BienViews;

public partial class BiensPage : ContentPage
{
    private readonly BiensPageViewModel viewModel;

    public BiensPage()
    {
        InitializeComponent();

        viewModel = new BiensPageViewModel();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        viewModel.LoadBiens();
    }

    private async Task OuvrirDetailBien(int bienId)
    {
        await Shell.Current.GoToAsync($"{nameof(DetailBienPage)}?bienId={bienId}");
    }

    private async void OnAjouterBienClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AjouterBienPage());
    }
}