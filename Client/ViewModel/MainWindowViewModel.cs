using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using Client.Model;
using Client.View;
using Client.Network;

namespace Client.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<FileModel> Files { get; private set; }
        public bool IgnoreSymbols { get; set; } = false;
        private FileModel? _selectedFile = null;
        public FileModel? SelectedFile
        {
            get => _selectedFile;
            set
            {
                _selectedFile = value;
                OnPropertyChanged();
            }
        }

        public ICommand OpenServerSelectorCommand { get; }
        public ICommand ClearFilesCommand { get; }
        public ICommand AddFileCommand { get; }
        public ICommand RemoveSelectedFileCommand { get; }
        public ICommand SendToServerCommand { get; }


        public MainWindowViewModel()
        {
            Files = new ObservableCollection<FileModel>();

            // Команды меню
            OpenServerSelectorCommand = new RelayCommand(p => OpenServerSelector());
            ClearFilesCommand = new RelayCommand(p => ClearFiles(), p => Files.Count > 0);

            // Редактирование списка файлов
            AddFileCommand = new RelayCommand(p => AddFiles());
            RemoveSelectedFileCommand = new RelayCommand(obj => RemoveFile(obj), obj => obj is FileModel);

            // Отправка файлов на сервер
            SendToServerCommand = new RelayCommand(p => SendFiles(), p => Files.Count > 0);
        }


        private void OpenServerSelector() => new ServerSelectorView().ShowDialog();

        private void ClearFiles() => Files.Clear();

        private void AddFiles()
        {
            OpenFileDialog openFileDialog = new()
            {
                Multiselect = true,
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                foreach (var filePath in openFileDialog.FileNames)
                {
                    var icon = Icon.ExtractAssociatedIcon(filePath);
                    ImageSource imageSource = Imaging.CreateBitmapSourceFromHIcon(
                        icon.Handle,
                        Int32Rect.Empty,
                        BitmapSizeOptions.FromEmptyOptions());
                    Files.Add(new FileModel(imageSource, filePath));
                }
            }
        }

        private void RemoveFile(object obj)
        {
            if (obj is FileModel model)
            {
                Files.Remove(model);
                if (Files.Count > 0) SelectedFile = Files.Last();
            }
        }

        private async Task SendFiles()
        {
            var tasks = Files.Select(f => new PalindromeCheck(f, IgnoreSymbols).Check());
            await Task.WhenAll(tasks);
        }
    }
}
