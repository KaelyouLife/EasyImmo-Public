using BU.Services;
using EasyImmoMAUI.ViewModels.BiensViewModels;

namespace EasyImmoMAUI.Views;

[QueryProperty(nameof(BienId), "bienId")]
public partial class DetailBienPage : ContentPage
{
    private readonly Dictionary<string, (Label Label, BoxView Indicator, View Content)> _tabs;

    private readonly DetailBienViewModel viewModel;

    private int bienId;

    public string BienId
    {
        set
        {
            if (int.TryParse(value, out int id))
            {
                bienId = id;
                ChargerBien();
            }
        }
    }

    public DetailBienPage()
    {
        InitializeComponent();

        viewModel = new DetailBienViewModel();
        BindingContext = viewModel;

        _tabs = new Dictionary<string, (Label, BoxView, View)>
        {
            ["Informations"] = (TabLabelInformations, TabIndicatorInformations, ContentInformations),
            ["Documents"] = (TabLabelDocuments, TabIndicatorDocuments, ContentDocuments),
            ["Evenements"] = (TabLabelEvenements, TabIndicatorEvenements, ContentEvenements),
            ["Contacts"] = (TabLabelContacts, TabIndicatorContacts, ContentContacts),
            ["Photos"] = (TabLabelPhotos, TabIndicatorPhotos, ContentPhotos),
            ["Historique"] = (TabLabelHistorique, TabIndicatorHistorique, ContentHistorique),
        };
    }

    private async void ChargerBien()
    {
        await viewModel.ChargerBienAsync(bienId);
    }

    private void OnTabTapped(object sender, TappedEventArgs e)
    {
        if (e.Parameter is string tabKey)
            SelectTab(tabKey);
    }

    private void SelectTab(string tabKey)
    {
        foreach (var (key, (label, indicator, content)) in _tabs)
        {
            bool isActive = key == tabKey;

            label.TextColor = isActive
                ? Color.FromArgb("#4F63FF")
                : Color.FromArgb("#8F9098");

            label.FontAttributes = isActive
                ? FontAttributes.Bold
                : FontAttributes.None;

            indicator.IsVisible = isActive;
            content.IsVisible = isActive;
        }
    }

    private async void OnEditerClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"EditerBienPage?bienId={bienId}");
    }

    private async void OnSupprimerClicked(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert(
            "Supprimer ce bien",
            "Êtes-vous sûr de vouloir supprimer ce bien ? Cette action est irréversible.",
            "Supprimer",
            "Annuler");

        if (confirm)
        {
            var result = new BienService().DeleteBien(bienId);

            if (result.Success)
                await Shell.Current.GoToAsync("..");
            else
                await DisplayAlert("Erreur", result.ErrorMessage, "OK");
        }
    }

    private async void OnAjouterDocumentClicked(object sender, EventArgs e)
    {
        var result = await viewModel.DocumentsViewModel.AjouterDocument();
        if (!result.Success)
            await DisplayAlert("Erreur", result.ErrorMessage, "OK");
    }

    private async void OnSupprimerDocumentClicked(object sender, TappedEventArgs e)
    {
        if (e.Parameter is not int documentId) return;

        bool confirm = await DisplayAlert("Supprimer", "Supprimer ce document ?", "Supprimer", "Annuler");
        if (!confirm) return;

        var result = viewModel.DocumentsViewModel.SupprimerDocument(documentId);
        if (!result.Success)
            await DisplayAlert("Erreur", result.ErrorMessage, "OK");
    }

    private async void OnOuvrirDocumentClicked(object sender, TappedEventArgs e)
    {
        if (e.Parameter is not string chemin) return;

        if (!File.Exists(chemin))
        {
            await DisplayAlert("Erreur", "Le fichier est introuvable.", "OK");
            return;
        }

        await Launcher.OpenAsync(new OpenFileRequest
        {
            File = new ReadOnlyFile(chemin)
        });
    }

    private async void OnAjouterContactClicked(object sender, EventArgs e)
    {
        var result = viewModel.ContactsViewModel.AjouterContact();
        if (!result.Success)
            await DisplayAlert("Erreur", result.ErrorMessage, "OK");
    }

    private async void OnSupprimerContactClicked(object sender, TappedEventArgs e)
    {
        if (e.Parameter is not int relationId) return;

        bool confirm = await DisplayAlert("Supprimer", "Retirer ce contact du bien ?", "Supprimer", "Annuler");
        if (!confirm) return;

        var result = viewModel.ContactsViewModel.SupprimerContact(relationId);
        if (!result.Success)
            await DisplayAlert("Erreur", result.ErrorMessage, "OK");
    }

    private async void OnAjouterPhotosClicked(object sender, TappedEventArgs e)
    {
        var result = await viewModel.PhotosViewModel.AjouterPhotos();
        if (!result.Success)
            await DisplayAlert("Erreur", result.ErrorMessage, "OK");
    }

    private async void OnPhotoClicked(object sender, TappedEventArgs e)
    {
        if (e.Parameter is not int photoId) return;
        var result = viewModel.PhotosViewModel.DefinirCommePrincipale(photoId);
        if (!result.Success)
            await DisplayAlert("Erreur", result.ErrorMessage, "OK");
    }

    private async void OnSupprimerPhotoClicked(object sender, TappedEventArgs e)
    {
        if (e.Parameter is not int photoId) return;
        bool confirm = await DisplayAlert("Supprimer", "Supprimer cette photo ?", "Supprimer", "Annuler");
        if (!confirm) return;
        var result = viewModel.PhotosViewModel.SupprimerPhoto(photoId);
        if (!result.Success)
            await DisplayAlert("Erreur", result.ErrorMessage, "OK");
    }

    private async void OnSupprimerToutesPhotosClicked(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert("Supprimer", "Supprimer toutes les photos ?", "Supprimer", "Annuler");
        if (!confirm) return;
        var result = viewModel.PhotosViewModel.SupprimerToutesLesPhotos();
        if (!result.Success)
            await DisplayAlert("Erreur", result.ErrorMessage, "OK");
    }
}