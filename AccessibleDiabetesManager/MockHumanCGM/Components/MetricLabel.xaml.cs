namespace MockHumanCGM.Components;

public partial class MetricLabel : ContentView
{
    public static readonly BindableProperty MetricTitleProperty = BindableProperty.Create(nameof(MetricTitle), typeof(string), typeof(MetricLabel), string.Empty);

    public static readonly BindableProperty MetricValueProperty = BindableProperty.Create(nameof(MetricValue), typeof(string), typeof(MetricLabel), string.Empty);

    public string MetricTitle {
        get => (string)GetValue(MetricTitleProperty);
        set => SetValue(MetricTitleProperty, value);
    }
    public string MetricValue
    {
        get => (string)GetValue(MetricValueProperty);
        set => SetValue(MetricValueProperty, value);
    }

    public MetricLabel()
	{
		InitializeComponent();
	}
}