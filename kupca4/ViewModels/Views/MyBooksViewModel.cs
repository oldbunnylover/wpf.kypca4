using kupca4.DB;
using kupca4.Helpers.Commands;
using kupca4.ViewModels.Base;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace kupca4.ViewModels.Views
{
    class MyBooksViewModel : ViewModel
    {
        #region private fields

        private readonly User user;
        private readonly KP_LibraryContext context = new KP_LibraryContext();
        private readonly MainWindowViewModel MainWindowVM;

        private ObservableCollection<Book> _uploadedBooksList;
        private ObservableCollection<Book> _likedBooksList;
        private bool _uploadedBooksVisibility;

        #endregion

        #region public fields

        public ObservableCollection<Book> uploadedBooksList
        {
            get => _uploadedBooksList;
            set => Set(ref _uploadedBooksList, value);
        }

        public ObservableCollection<Book> likedBooksList
        {
            get => _likedBooksList;
            set => Set(ref _likedBooksList, value);
        }

        public bool uploadedBooksVisibility
        {
            get => _uploadedBooksVisibility;
            set => Set(ref _uploadedBooksVisibility, value);
        }

        #endregion
        #region commands

        public ICommand EditBookCommand { get; }
        private void OnEditBookCommandExecuted(object p)
        {
            try
            {
                MainWindowVM.selectedVM = new BookUploadViewModel(user, MainWindowVM, (int)p);
            }
            catch
            {
                MainWindowVM.dialogText = "Отсутствует подключение к интернету.";
                MainWindowVM.dialog = true;
            }
        }

        public ICommand HideBookCommand { get; }
        private void OnHideBookCommandExecuted(object p)
        {
            try
            {
                context.Books.Find((int)p).Hidden = true;
                context.SaveChanges();
                uploadedBooksList = new ObservableCollection<Book>(context.Books.Where(b => b.User == user));
            }
            catch
            {
                MainWindowVM.dialogText = "Отсутствует подключение к интернету.";
                MainWindowVM.dialog = true;
            }
        }

        public ICommand ShowBookCommand { get; }
        private void OnShowBookCommandExecuted(object p)
        {
            try
            {
                context.Books.Find((int)p).Hidden = false;
                context.SaveChanges();
                uploadedBooksList = new ObservableCollection<Book>(context.Books.Where(b => b.User == user));
            }
            catch
            {
                MainWindowVM.dialogText = "Отсутствует подключение к интернету.";
                MainWindowVM.dialog = true;
            }
        }

        public ICommand RemoveFavoriteCommand { get; }
        private void OnRemoveFavoriteCommandExecuted(object p)
        {
            try
            {
                context.SavedBooks.Remove(context.SavedBooks.Find(user.Username, (int)p));
                context.SaveChanges();
                likedBooksList = new ObservableCollection<Book>(context.Books.Where(b => context.SavedBooks.Where(s => s.Username == user.Username).Select(s => s.BookId).Contains(b.BookId)));
            }
            catch
            {
                MainWindowVM.dialogText = "Отсутствует подключение к интернету.";
                MainWindowVM.dialog = true;
            }
        }

        public ICommand SwitchViewCommand { get; }
        private void OnSwitchViewCommandExecuted(object p)
        {
            try
            {
                MainWindowVM.selectedVM = new SelectedBookViewModel(user, (int)p, MainWindowVM, this);
            }
            catch
            {
                MainWindowVM.dialogText = "Отсутствует подключение к интернету.";
                MainWindowVM.dialog = true;
            }
        }

        #endregion

        public MyBooksViewModel(User user, MainWindowViewModel vm)
        {
            this.user = user;
            MainWindowVM = vm;
            likedBooksList = new ObservableCollection<Book>(context.Books.Where(b => context.SavedBooks.Where(s => s.Username == user.Username).Select(s => s.BookId).Contains(b.BookId)));
            uploadedBooksList = new ObservableCollection<Book>(context.Books.Where(b => b.User == user));

            _uploadedBooksVisibility = _uploadedBooksList.Count() > 0;

            EditBookCommand = new LambdaCommand(OnEditBookCommandExecuted);
            HideBookCommand = new LambdaCommand(OnHideBookCommandExecuted);
            ShowBookCommand = new LambdaCommand(OnShowBookCommandExecuted);
            SwitchViewCommand = new LambdaCommand(OnSwitchViewCommandExecuted);
            RemoveFavoriteCommand = new LambdaCommand(OnRemoveFavoriteCommandExecuted);
        }
    }
}
