using System.Windows;
using System.Windows.Input;
using kupca4.Helpers.Commands;
using kupca4.ViewModels.Base;

namespace kupca4.ViewModels
{
    class LoginRegisterViewModel : ViewModel
    {
        #region Commands

        //public ICommand AppCloseCommand { get; }
        //private bool CanAppCloseCommandExecute(object p) => true;

        //private void OnAppCloseCommandExecuted(object p)
        //{
        //    Application.Current.Shutdown();
        //}

        #endregion

        public LoginRegisterViewModel()
        {
            #region Commands

            //AppCloseCommand = new LambdaCommand(OnAppCloseCommandExecuted, CanAppCloseCommandExecute);

            #endregion
        }
    }
}
