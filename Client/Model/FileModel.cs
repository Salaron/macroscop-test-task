using System.IO;
using System.Windows.Media;

using Client.ViewModel;

namespace Client.Model
{
    public enum FileStatus
    {
        Error,
        Loading,
        Palindrome,
        NotPalindrome,
        Overload
    }
    public class FileModel : ViewModelBase
    {
        public ImageSource FileIcon { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }

        private string _status;
        public string Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged();
            }
        }

        public FileModel(ImageSource icon, string path)
        {
            FileIcon = icon;
            FilePath = path;
            FileName = path.Substring(path.LastIndexOf("\\") + 1);
            Status = "Неизвестно";
        }

        public string GetContent()
        {
            string content = "";
            using(StreamReader r = new(FilePath))
            {
                content = r.ReadToEnd();
            }
            
            return content;
        }

        public void UpdateStatus(FileStatus status)
        {
            switch (status)
            {
                case FileStatus.Loading:
                    Status = "Загрузка...";
                    break;
                case FileStatus.Error:
                    Status = "Ошибка";
                    break;
                case FileStatus.NotPalindrome:
                    Status = "Не палиндром";
                    break;
                case FileStatus.Palindrome:
                    Status = "Палиндром";
                    break;
                case FileStatus.Overload:
                    Status = "Сервер перегружен";
                    break;
            }
        }
    }
}
