using System.Windows.Input;

namespace EasyImmoMAUI.Views.Components.Buttons;

public partial class SecondaryButton : ContentView
{
    public SecondaryButton()
    {
        InitializeComponent();
    }

    public event EventHandler? Clicked;

    public static readonly BindableProperty ButtonTextProperty =
        BindableProperty.Create(
            nameof(ButtonText),
            typeof(string),
            typeof(SecondaryButton),
            string.Empty);

    public string ButtonText
    {
        get => (string)GetValue(ButtonTextProperty);
        set => SetValue(ButtonTextProperty, value);
    }



    private void OnButtonClicked(object sender, EventArgs e)
    => Clicked?.Invoke(this, e);
}