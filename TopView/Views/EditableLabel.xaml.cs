namespace TopView.Views;

public partial class EditableLabel : ContentView
{
    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(
            nameof(Text),
            typeof(string),
            typeof(EditableLabel),
            defaultBindingMode: BindingMode.TwoWay);

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public static readonly BindableProperty FontSizeProperty =
        BindableProperty.Create(
            nameof(FontSize),
            typeof(double),
            typeof(EditableLabel),
            14.0, // default font size
            propertyChanged: OnFontSizeChanged);

    public double FontSize
    {
        get => (double)GetValue(FontSizeProperty);
        set => SetValue(FontSizeProperty, value);
    }

    bool _isEditing = false;

    public EditableLabel()
    {
        InitializeComponent();

        // Hover effects for desktop
#if WINDOWS || MACCATALYST
        var hoverGesture = new PointerGestureRecognizer();
        hoverGesture.PointerEntered += (s, e) => ShowEditIcon(true);
        hoverGesture.PointerExited += (s, e) => ShowEditIcon(false);
        DisplayPanel.GestureRecognizers.Add(hoverGesture);
#endif
    }

    private void ShowEditIcon(bool show)
    {
        EditIcon.FadeTo(show ? 1 : 0, 150);
    }

    private void OnEditTapped(object sender, TappedEventArgs e)
    {
        _isEditing = true;
        DisplayPanel.IsVisible = false;
        EditEntry.IsVisible = true;

        EditEntry.Focus();
        EditEntry.SelectionLength = EditEntry.Text?.Length ?? 0;
        EditEntry.CursorPosition = 0;
    }

    private void OnEntryCompleted(object sender, EventArgs e)
    {
        CommitEdit();
    }

    private void OnEntryUnfocused(object sender, FocusEventArgs e)
    {
        if (_isEditing)
            CommitEdit();
    }

    private void CommitEdit()
    {
        _isEditing = false;
        DisplayPanel.IsVisible = true;
        EditEntry.IsVisible = false;
        ShowEditIcon(false);
    }
    private static void OnFontSizeChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is EditableLabel control)
        {
            control.DisplayLabel.FontSize = (double)newValue;
            control.EditEntry.FontSize = (double)newValue;
        }
    }
}