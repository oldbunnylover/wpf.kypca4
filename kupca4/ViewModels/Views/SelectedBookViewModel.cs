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
        private readonly ViewModel beforeVM;
        
        private int _rate;
        private bool _userRatePanelVisibility;
        private Book _selectedBook;

        #endregion

        #region public fields

        public Book selectedBook
        {
            get => _selectedBook;
        }

        public int rate
        {
            get => _rate;
            set
            {
                try
                {
                    Set(ref _rate, value);
                    if (context.BookRates.FirstOrDefault(b => b.BookId == _selectedBook.BookId && b.Username == user.Username) != null)
                    {
                        context.BookRates.Find(user.Username, _selectedBook.BookId).UserRate = rate;
                    }
                    else
                    {
                        context.BookRates.Add(new BookRates { Username = user.Username, BookId = _selectedBook.BookId, UserRate = rate });
                    }
                    context.SaveChanges();
                }
                catch
                {
                    mainWindowVM.dialogText = "Отсутствует подключение к интернету.";
                    mainWindowVM.dialog = true;
                }
            }
        }

        public bool userRatePanelVisibility
        {
            get => _userRatePanelVisibility;
            set => Set(ref _userRatePanelVisibility, value);
        }

        #endregion

        #region commands

        public ICommand SwitchViewCommand { get; }
        private bool CanSwitchViewCommandExecute(object p) => true;
        private void OnSwitchViewCommandExecuted(object p)
        {
            try
            {
                context.Users.FirstOrDefault(u => u.Username == user.Username);
                mainWindowVM.selectedVM = beforeVM;
            }
            catch
            {
                mainWindowVM.dialogText = "Отсутствует подключение к интернету.";
                mainWindowVM.dialog = true;
            }
        } 

        public ICommand ToFavoritesCommand { get; }
        private void OnToFavoritesCommandExecuted(object p)
        {
            try
            {
                if(context.SavedBooks.FirstOrDefault(s => s.BookId == _selectedBook.BookId && s.Username == user.Username) == null)
                {
                    context.SavedBooks.Add(new SavedBook { BookId = selectedBook.BookId, Username = user.Username });
                    context.SaveChanges();
                    userRatePanelVisibility = true;
                }
            }
            catch
            {
                mainWindowVM.dialogText = "Отсутствует подключение к интернету.";
                mainWindowVM.dialog = true;
            }
        }

        public ICommand ReadBookCommand { get; }
        private bool CanReadBookCommandExecute(object p) => true;
        private void OnReadBookCommandExecuted(object p)
        {
            try
            {
                var ReaderViewModel = new ReaderViewModel(_selectedBook);
                var Reader = new Reader
                {
                    DataContext = ReaderViewModel
                };
                Reader.Show();
            }
            catch
            {
                mainWindowVM.dialogText = "Отсутствует подключение к интернету.";
                mainWindowVM.dialog = true;
            }
        }

        #endregion

        public SelectedBookViewModel(User user, int bookID, MainWindowViewModel vm, ViewModel beforeVM)
        {
            this.user = user;
            mainWindowVM = vm;
            this.beforeVM = beforeVM;

            _selectedBook = context.Books.Find(bookID);
            _userRatePanelVisibility = context.SavedBooks.FirstOrDefault(s => s.BookId == _selectedBook.BookId && s.Username == user.Username) != null;

            if (context.BookRates.FirstOrDefault(b => b.BookId == bookID && b.Username == user.Username) != null)
            {
                _rate = context.BookRates.Find(user.Username, bookID).UserRate.Value;
            } else
            {
                _rate = 0;
            }

            SwitchViewCommand = new LambdaCommand(OnSwitchViewCommandExecuted, CanSwitchViewCommandExecute);
            ToFavoritesCommand = new LambdaCommand(OnToFavoritesCommandExecuted);
            ReadBookCommand = new LambdaCommand(OnReadBookCommandExecuted, CanReadBookCommandExecute);
        }
    }
}
