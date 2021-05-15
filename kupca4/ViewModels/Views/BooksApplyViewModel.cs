using kupca4.DB;
using kupca4.Helpers.Commands;
using kupca4.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace kupca4.ViewModels.Views
{
    class BooksApplyViewModel : ViewModel
    {
        #region private fields

        private readonly KP_LibraryContext context = new KP_LibraryContext();
        private readonly MainWindowViewModel MainVM;
        private readonly User user;

        private ObservableCollection<Book> _newBooksList;

        #endregion

        #region public fields

        public ObservableCollection<Book> newBooksList
        {
            get => _newBooksList;
            set => Set(ref _newBooksList, value);
        }

        #endregion

        #region commands

        public ICommand ApplyBookCommand { get; }
        private void OnApplyBookCommandExecuted(object p)
        {
            context.Books.Find((int)p).Applied = BookStatus.Applied;
            context.SaveChanges();
            newBooksList = new ObservableCollection<Book>(context.Books.Where(b => context.Users.Where(u => u.Blocked == false).Select(u => u.Username).Contains(b.AuthorName) && b.Applied == BookStatus.NeedModer));
        }

        public ICommand DeclineBookCommand { get; }
        private void OnDeclineBookCommandExecuted(object p)
        {
            context.Books.Find((int)p).Applied = BookStatus.Canceled;
            context.SaveChanges();
            newBooksList = new ObservableCollection<Book>(context.Books.Where(b => context.Users.Where(u => u.Blocked == false).Select(u => u.Username).Contains(b.AuthorName) && b.Applied == BookStatus.NeedModer));
        }

        public ICommand MoreInfoCommand { get; }
        private void OnMoreInfoCommandExecuted(object p)
        {
            MainVM.selectedVM = new SelectedBookViewModel(user, (int)p, MainVM, new UserViewModel(user, MainVM, this));
        }

        public ICommand AuthorBlockCommand { get; }
        private void OnAuthorBlockCommandExecuted(object p)
        {
            context.Users.Find(context.Books.Find((int)p).AuthorName).Blocked = true;
            context.Books.Find((int)p).Applied = BookStatus.Banned;
            context.SaveChanges();
            newBooksList = new ObservableCollection<Book>(context.Books.Where(b => context.Users.Where(u => u.Blocked == false).Select(u => u.Username).Contains(b.AuthorName) && b.Applied == BookStatus.NeedModer));
        }

        #endregion

        public BooksApplyViewModel(MainWindowViewModel MainVM, User user)
        {
            this.user = user;
            this.MainVM = MainVM;

            _newBooksList = new ObservableCollection<Book>(context.Books.Where(b => context.Users.Where(u => u.Blocked == false).Select(u => u.Username).Contains(b.AuthorName) && b.Applied == BookStatus.NeedModer));
            ApplyBookCommand = new LambdaCommand(OnApplyBookCommandExecuted);
            DeclineBookCommand = new LambdaCommand(OnDeclineBookCommandExecuted);
            MoreInfoCommand = new LambdaCommand(OnMoreInfoCommandExecuted);
            AuthorBlockCommand = new LambdaCommand(OnAuthorBlockCommandExecuted);
        }
    }
}
