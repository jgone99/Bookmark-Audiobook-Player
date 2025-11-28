namespace Audiobookplayer.Pages;

public partial class PlayerPage : ContentPage
{
	public PlayerPage()
	{
		InitializeComponent();
	}

	private void OnSeekRequested(object sender, double position)
	{
		var viewModel = BindingContext as ViewModels.PlayerViewModel;
		viewModel?.SeekToCommand.Execute(position);
    }
}