using kupca4.DB;
using kupca4.Helpers.Commands;
using kupca4.ViewModels.Base;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Linq;

namespace kupca4.ViewModels.Views
{
    class MyBooksViewModel : ViewModel
    {
        #region private fields

        private readonly User user;
        private readonly KP_LibraryContext context = new KP_LibraryContext();

        private ObservableCollection<Book> _uploadedBooksList;
        private ObservableCollection<Book> _likedBooksList = new ObservableCollection<Book>();

        #endregion

        #region

        public ObservableCollection<Book> uploadedBooksList
        {
            get => _uploadedBooksList;
        }

        public ObservableCollection<Book> likedBooksList
        {
            get => _likedBooksList;
        }

        #endregion

        public MyBooksViewModel(User user)
        {
            this.user = user;

            _uploadedBooksList = new ObservableCollection<Book>(context.Books.Where(b => b.User == user));
            _likedBooksList = new ObservableCollection<Book>(context.Books.Where(b => context.SavedBooks.Where(s => s.Username == user.Username).Select(s => s.BookId).Contains(b.BookId)));
            
        }
    }
}
