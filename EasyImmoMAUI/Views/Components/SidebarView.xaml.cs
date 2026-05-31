namespace EasyImmoMAUI.Views.Components;

public partial class SidebarView : ContentView
{
    public SidebarView()
    {
        InitializeComponent();
    }

    public static readonly BindableProperty CurrentPageProperty =
    BindableProperty.Create(nameof(CurrentPage), typeof(string), typeof(SidebarView), "");

    public string CurrentPage
    {
        get => (string)GetValue(CurrentPageProperty);
        set => SetValue(CurrentPageProperty, value);
    }

    private async void OnDashboardClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//DashboardPage");
    }

    private async void OnBiensClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//BiensPage");
    }

    private async void OnEvenementsClicked(object sender, EventArgs e)
    {
            await Shell.Current.GoToAsync("//EvenementsPage");
    }
    
    private async void OnPersonnesClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//PersonnesPage");
    }

    private async void OnReglagesClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//ReglagesPage");
    }
}