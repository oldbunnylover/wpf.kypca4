using kupca4.DB;
using kupca4.ViewModels.Base;

namespace kupca4.ViewModels.Views
{
    class SelectedBookViewModel : ViewModel
    {
        #region private fields

        private readonly User user;
        private readonly KP_LibraryContext context = new KP_LibraryContext();

        private Book _selectedBook;

        #endregion

        #region public fields

        public Book selectedBook
        {
            get => _selectedBook;
        }

        #endregion

        #region commands



        #endregion

        public SelectedBookViewModel(User user, int bookID)
        {
            this.user = user;

            _selectedBook = context.Books.Find(bookID);
        }
    }
}
