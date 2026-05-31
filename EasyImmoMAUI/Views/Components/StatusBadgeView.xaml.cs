namespace EasyImmoMAUI.Views.Components;

public partial class StatusBadgeView : ContentView
{
    public StatusBadgeView()
    {
        InitializeComponent();
        UpdateColors();
    }

    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(
            nameof(Text),
            typeof(string),
            typeof(StatusBadgeView),
            string.Empty,
            propertyChanged: OnStatusChanged);

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    private static void OnStatusChanged(BindableObject bindable, object oldValue, object newValue)
    {
        ((StatusBadgeView)bindable).UpdateColors();
    }

    private void UpdateColors()
    {
        switch (Text)
        {
            case "En vente":
            case "À louer":
            case "Contrat":
            case "Rencontre":
                BadgeBorder.BackgroundColor = Color.FromArgb("#E6F5EF");
                BadgeLabel.TextColor = Color.FromArgb("#3E8A76");
                break;

            case "Vendu":
            case "Loué":
            case "Document":
            case "Visite":
                BadgeBorder.BackgroundColor = Color.FromArgb("#FFF4D6");
                BadgeLabel.TextColor = Color.FromArgb("#C58A00");
                break;

            default:
                BadgeBorder.BackgroundColor = Color.FromArgb("#ECECF2");
                BadgeLabel.TextColor = Color.FromArgb("#7A7C85");
                break;
        }
    }
}