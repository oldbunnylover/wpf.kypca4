using kupca4.Helpers.Commands;
using kupca4.ViewModels.Base;
using System.Windows;
using System.Windows.Input;

namespace kupca4.ViewModels
{
    class ErrorViewModel : ViewModel
    {
        private string errorMsg;

        public string ErrorMsg
        {
            get => errorMsg;
            set => Set(ref errorMsg, value);
        }

        public ICommand TryAgainCommand { get; }
        private void OnTryAgainCommandExecuted(object p) => (p as Window).Close();

        public ErrorViewModel(string error)
        {
            TryAgainCommand = new LambdaCommand(OnTryAgainCommandExecuted);
            errorMsg = error;
        }
    }
}
