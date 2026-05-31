namespace EasyImmoMAUI.Views.Components.Buttons;

public partial class DeleteButton : ContentView
{
    public DeleteButton()
    {
        InitializeComponent();
    }

    public event EventHandler? Clicked;

    private void OnButtonClicked(object sender, EventArgs e)
    => Clicked?.Invoke(this, e);

    public static readonly BindableProperty ButtonTextProperty =
        BindableProperty.Create(
            nameof(ButtonText),
            typeof(string),
            typeof(DeleteButton),
            string.Empty);

    public string ButtonText
    {
        get => (string)GetValue(ButtonTextProperty);
        set => SetValue(ButtonTextProperty, value);
    }
}