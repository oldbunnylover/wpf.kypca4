using kupca4.DB;
using kupca4.Helpers.Commands;
using kupca4.ViewModels.Base;
using System.Windows.Input;

namespace kupca4.ViewModels.Views
{
    class SelectedBookViewModel : ViewModel
    {
        #region private fields

        private readonly User user;
        private readonly KP_LibraryContext context = new KP_LibraryContext();
        private readonly MainWindowViewModel mainWindowVM;

        private Book _selectedBook;

        #endregion

        #region public fields

        public Book selectedBook
        {
            get => _selectedBook;
        }

        #endregion

        #region commands

        public ICommand SwitchViewCommand { get; }
        private bool CanSwitchViewCommandExecute(object p) => true;
        private void OnSwitchViewCommandExecuted(object p)
        {
            mainWindowVM.selectedVM = new AllBooksViewModel(user, mainWindowVM);
        }

        #endregion

        public SelectedBookViewModel(User user, int bookID, MainWindowViewModel vm)
        {
            this.user = user;
            mainWindowVM = vm;

            _selectedBook = context.Books.Find(bookID);

            SwitchViewCommand = new LambdaCommand(OnSwitchViewCommandExecuted, CanSwitchViewCommandExecute);
        }
    }
}
