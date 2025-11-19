using Audiobookplayer.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
namespace Audiobookplayer.ViewModels
{
    public partial class SettingsViewModel : ObservableObject
    {
        private const string LibraryPrefKey = "library_uri";

        public ICommand FolderPickerCommand { private set; get; }

        [ObservableProperty]
        private string libraryFolderDisplayPath;

        public SettingsViewModel()
        {
            string? libraryFolderUriString = FolderPickerService.GetSavedLibraryFolder(LibraryPrefKey);
            LibraryFolderDisplayPath = $"\"{FileSystemServices.GetDisplayPath(libraryFolderUriString) ?? string.Empty}\"";
            FolderPickerCommand = new AsyncRelayCommand(SelectLibraryFolderAsync);
        }

        private async Task SelectLibraryFolderAsync()
        {
            string? uriString = await FileSystemServices.FolderPickerService.PickFolderAsync(LibraryPrefKey);

            if (!string.IsNullOrEmpty(uriString))
            {
                FolderPickerService.SaveLibraryFolder(LibraryPrefKey, uriString);
                LibraryFolderDisplayPath =
                    $"\"{FileSystemServices.GetDisplayPath(uriString) ?? string.Empty}\"";
                FileSystemServices.NotifyLibraryFolderChanged();
            }
        }
    }
}
