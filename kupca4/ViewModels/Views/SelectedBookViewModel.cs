using kupca4.DB;
using kupca4.Helpers.Commands;
using kupca4.ViewModels.Base;
using System.Windows.Input;
using System.Linq;

namespace kupca4.ViewModels.Views
{
    class SelectedBookViewModel : ViewModel
    {
        #region private fields

        private readonly User user;
        private readonly KP_LibraryContext context = new KP_LibraryContext();
        private readonly MainWindowViewModel mainWindowVM;
        private readonly bool fromMyBooks;
        private string savedSorting;

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
            if (fromMyBooks)
            {
                mainWindowVM.selectedVM = new MyBooksViewModel(user, mainWindowVM);
            }
            else
            {
                mainWindowVM.selectedVM = new AllBooksViewModel(user, mainWindowVM, savedSorting);
            }
        } 

        public ICommand ToFavoritesCommand { get; }
        private bool CanToFavoritesCommandExecute(object p) => context.SavedBooks.FirstOrDefault(s => s.BookId == _selectedBook.BookId && s.Username == user.Username) == null;
        private void OnToFavoritesCommandExecuted(object p)
        {
            context.SavedBooks.Add(new SavedBook { BookId = selectedBook.BookId, Username = user.Username });
            context.SaveChanges();
        }

        public ICommand ReadBookCommand { get; }
        private bool CanReadBookCommandExecute(object p) => true;
        private void OnReadBookCommandExecuted(object p)
        {
            var ReaderViewModel = new ReaderViewModel(_selectedBook);
            var Reader = new Reader
            {
                DataContext = ReaderViewModel
            };
            Reader.Show();
        }

        #endregion

        public SelectedBookViewModel(User user, int bookID, MainWindowViewModel vm, bool fromMyBooks = false, string savedSorting = "по новизне")
        {
            this.user = user;
            mainWindowVM = vm;
            this.fromMyBooks = fromMyBooks;
            this.savedSorting = savedSorting;

            _selectedBook = context.Books.Find(bookID);

            SwitchViewCommand = new LambdaCommand(OnSwitchViewCommandExecuted, CanSwitchViewCommandExecute);
            ToFavoritesCommand = new LambdaCommand(OnToFavoritesCommandExecuted, CanToFavoritesCommandExecute);
            ReadBookCommand = new LambdaCommand(OnReadBookCommandExecuted, CanReadBookCommandExecute);
        }
    }
}
