using kupca4.DB;
using kupca4.Helpers.Commands;
using kupca4.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace kupca4.ViewModels.Views
{
    class MyBooksViewModel : ViewModel
    {
        #region private fields

        private readonly User user;
        private readonly KP_LibraryContext context = new KP_LibraryContext();

        private ObservableCollection<Book> _uploadedBooksList;

        #endregion

        #region

        public ObservableCollection<Book> uploadedBooksList
        {
            get => _uploadedBooksList;
        }

        #endregion

        public MyBooksViewModel(User user)
        {
            this.user = user;

            _uploadedBooksList = new ObservableCollection<Book>(context.Books.Where(b => b.User == user));
        }
    }
}
