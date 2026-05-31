using System.Windows.Input;

namespace EasyImmoMAUI.Views.Components;

public partial class BienCardView : ContentView
{
    public BienCardView()
    {
        InitializeComponent();
    }

    public static readonly BindableProperty PhotoSourceProperty =
        BindableProperty.Create(nameof(PhotoSource), typeof(string), typeof(BienCardView), string.Empty);

    public string PhotoSource
    {
        get => (string)GetValue(PhotoSourceProperty);
        set => SetValue(PhotoSourceProperty, value);
    }

    public static readonly BindableProperty TitreAnnonceProperty =
        BindableProperty.Create(nameof(TitreAnnonce), typeof(string), typeof(BienCardView), string.Empty);

    public string TitreAnnonce
    {
        get => (string)GetValue(TitreAnnonceProperty);
        set => SetValue(TitreAnnonceProperty, value);
    }

    public static readonly BindableProperty TypeBienProperty =
        BindableProperty.Create(nameof(TypeBien), typeof(string), typeof(BienCardView), string.Empty);

    public string TypeBien
    {
        get => (string)GetValue(TypeBienProperty);
        set => SetValue(TypeBienProperty, value);
    }

    public static readonly BindableProperty CodePostalProperty =
        BindableProperty.Create(nameof(CodePostal), typeof(string), typeof(BienCardView), string.Empty);

    public string CodePostal
    {
        get => (string)GetValue(CodePostalProperty);
        set => SetValue(CodePostalProperty, value);
    }

    public static readonly BindableProperty CommuneProperty =
    BindableProperty.Create(nameof(Commune), typeof(string), typeof(BienCardView), string.Empty);

    public string Commune
    {
        get => (string)GetValue(CommuneProperty);
        set => SetValue(CommuneProperty, value);
    }

    public static readonly BindableProperty StatutProperty =
        BindableProperty.Create(nameof(Statut), typeof(string), typeof(BienCardView), string.Empty);

    public string Statut
    {
        get => (string)GetValue(StatutProperty);
        set => SetValue(StatutProperty, value);
    }

    public static readonly BindableProperty CommandProperty =
    BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(BienCardView));

    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public static readonly BindableProperty BienIdProperty =
        BindableProperty.Create(nameof(BienId), typeof(int), typeof(BienCardView), 0);

    public int BienId
    {
        get => (int)GetValue(BienIdProperty);
        set => SetValue(BienIdProperty, value);
    }
}