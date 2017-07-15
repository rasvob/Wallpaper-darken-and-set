using System.ComponentModel;
using System.Runtime.CompilerServices;
using WallSetter_v2.Annotations;

namespace WallSetter_v2.ViewModels
{
    public class CommonDialogViewModel: INotifyPropertyChanged
    {
        private string _dialogTitle;
        private string _message;

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

        public string Message
        {
            get => _message;
            set
            {
                if (value == _message) return;
                _message = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}