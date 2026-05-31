using EasyImmoMAUI.Views;
using EasyImmoMAUI.Views.PersonnesView;

namespace EasyImmoMAUI;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(AjouterBienPage), typeof(AjouterBienPage));
        Routing.RegisterRoute(nameof(DetailBienPage), typeof(DetailBienPage));
        Routing.RegisterRoute(nameof(EditerBienPage), typeof(EditerBienPage));
        Routing.RegisterRoute(nameof(AjouterPersonnePage), typeof(AjouterPersonnePage));
        Routing.RegisterRoute(nameof(EditerPersonnePage), typeof(EditerPersonnePage));
    }
}