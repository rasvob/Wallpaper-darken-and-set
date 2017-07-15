using System.ComponentModel;
using System.Runtime.CompilerServices;
using WallSetter_v2.Annotations;

namespace WallSetter_v2.ViewModels
{
    public class DownloadDialogViewModel: INotifyPropertyChanged, IDataErrorInfo
    {
        private string _dialogTitle;
        private string _link;

        public string DialogTitle
        {
            get => _dialogTitle;
            set
            {
                if (value == _dialogTitle) return;
                _dialogTitle = value;
                OnPropertyChanged();
            }
        }

        public string Link
        {
            get => _link;
            set
            {
                if (value == _link) return;
                _link = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CanBeAcceptted));
            }
        }

        public bool CanBeAcceptted => this[nameof(Link)] == string.Empty;

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case nameof(Link):
                        if (string.IsNullOrWhiteSpace(Link))
                        {
                            return "Field is required";
                        }
                        return string.Empty;
                }
                return string.Empty;
            }
        }

        public string Error => string.Empty;
    }
}