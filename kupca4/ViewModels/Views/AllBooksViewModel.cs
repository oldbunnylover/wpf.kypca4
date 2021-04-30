using kupca4.DB;
using kupca4.ViewModels.Base;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace kupca4.ViewModels.Views
{
    class AllBooksViewModel : ViewModel
    {
        #region private fields

        private readonly User user;
        private readonly KP_LibraryContext context = new KP_LibraryContext();
        private readonly List<string> _sorting = new List<string>{ "по новизне", "по алфавиту", "по популярности" };
        private string _sortingSelected;
        private ObservableCollection<Book> _booksList = new ObservableCollection<Book>((new KP_LibraryContext()).Books);

        #endregion

        #region public fields

        public List<string> sorting
        {
            get => _sorting;
        }

        public string sortingSelected
        {
            get => _sortingSelected;
            set => Set(ref _sortingSelected, value);
        }

        public ObservableCollection<Book> booksList
        {
            get => _booksList;
        }

        #endregion

        #region commands



        #endregion


        public AllBooksViewModel(User user)
        {
            this.user = user;
            _sortingSelected = _sorting[0];
        }
    }
}
