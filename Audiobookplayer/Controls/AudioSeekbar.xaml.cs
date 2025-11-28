namespace Audiobookplayer.Controls;

public partial class AudioSeekbar : ContentView
{
	public AudioSeekbar()
	{
		InitializeComponent();
	}

    public static readonly BindableProperty DurationProperty =
    BindableProperty.Create(nameof(Duration), typeof(double), typeof(AudioSeekbar), 1.0);

    public double Duration
    {
        get => (double)GetValue(DurationProperty);
        set => SetValue(DurationProperty, value);
    }

    public static readonly BindableProperty PositionProperty =
        BindableProperty.Create(nameof(Position), typeof(double), typeof(AudioSeekbar), 0.0, BindingMode.TwoWay);

    public double Position
    {
        get => (double)GetValue(PositionProperty);
        set => SetValue(PositionProperty, value);
    }

    bool _isDragging;

    public event EventHandler<double> SeekRequested;

    private void OnDragStarted(object sender, EventArgs e)
    {
        _isDragging = true;
    }

    private void OnDragCompleted(object sender, EventArgs e)
    {
        _isDragging = false;
        SeekRequested?.Invoke(this, Position);
    }
}