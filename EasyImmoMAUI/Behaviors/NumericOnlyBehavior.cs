namespace EasyImmoMAUI.Behaviors;

class NumericOnlyBehavior : Behavior<Entry>
{
    protected override void OnAttachedTo(Entry bindable)
    {
        bindable.TextChanged += OnEntryTextChanged;
        base.OnAttachedTo(bindable);
    }

    protected override void OnDetachingFrom(Entry bindable)
    {
        bindable.TextChanged -= OnEntryTextChanged;
        base.OnDetachingFrom(bindable);
    }

    private void OnEntryTextChanged(object sender, TextChangedEventArgs e)
    {
        var entry = (Entry)sender;
        var newText = e.NewTextValue;

        var filtered = new string(newText
            .Where((c, i) => char.IsDigit(c) ||
                             ((c == ',' || c == '.') && newText.IndexOfAny(new[] { ',', '.' }) == i))
            .ToArray());

        if (filtered != newText)
            entry.Text = filtered;
    }
}
