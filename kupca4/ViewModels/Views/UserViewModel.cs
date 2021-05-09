using kupca4.DB;
using kupca4.Helpers.Commands;
using kupca4.ViewModels.Base;
using System.Windows.Input;

namespace kupca4.ViewModels.Views
{
    class UserViewModel : ViewModel
    {
        #region private fields

        private readonly User user;
        private ViewModel _selectedVM;

        #endregion

        #region public fields

        public ViewModel selectedVM
        {
            get => _selectedVM;
            set => Set(ref _selectedVM, value);
        }

        #endregion

        #region commands

        public ICommand SwitchViewCommand { get; }
        private void OnSwitchViewCommandExecuted(object p)
        {
            switch (p.ToString())
            {
                case "BooksApply":
                    break;
                case "AuthorBlock":
                    break;
                case "NewModerator":
                    break;
            }
        }

        #endregion

        public UserViewModel(User user)
        {
            this.user = user;

            SwitchViewCommand = new LambdaCommand(OnSwitchViewCommandExecuted);
        }
    }
}
