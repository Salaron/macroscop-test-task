using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Client.Interfaces;

namespace Client.ViewModel
{
    public class ServerSelectorViewModel : ViewModelBase
    {
        private string _serverUrl = "";
        public string ServerUrl
        {
            get => _serverUrl;
            set
            {
                _serverUrl = value;
                OnPropertyChanged();
            }
        }

        public ICommand SaveAndCloseCommand { get; }

        public ServerSelectorViewModel()
        {
            ServerUrl = Settings.ServerUrl;
            SaveAndCloseCommand = new RelayCommand(obj => SaveAndClose(obj));
        }

        private void SaveAndClose(object obj)
        {
            if (obj is IClosable closableObj)
            {
                Settings.ServerUrl = ServerUrl;
                closableObj.Close();
            }
        }
    }
}
