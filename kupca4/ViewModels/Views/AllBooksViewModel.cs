using kupca4.DB;
using kupca4.Helpers.Commands;
using kupca4.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace kupca4.ViewModels.Views
{
    class AllBooksViewModel : ViewModel
    {
        #region private fields

        private readonly User user;
        private readonly KP_LibraryContext context = new KP_LibraryContext();
        private readonly List<string> _sorting = new List<string>{ "по новизне", "по алфавиту", "по популярности" };
        private readonly MainWindowViewModel parentVM;

        private string _sortingSelected;
        private ObservableCollection<Book> _booksList;

        #endregion

        #region public fields

        public List<string> sorting
        {
            get => _sorting;
        }

        public string sortingSelected
        {
            get => _sortingSelected;
            set
            {
                Set(ref _sortingSelected, value);
                switch (sortingSelected)
                {
                    case "по новизне":
                        booksList = new ObservableCollection<Book>(context.Books.OrderByDescending(b => b.BookId));
                        break;
                    case "по алфавиту":
                        booksList = new ObservableCollection<Book>(context.Books.OrderBy(b => b.Bookname));
                        break;
                    case "по популярности":
                        booksList = new ObservableCollection<Book>(context.Books.OrderBy(b => b.Rate));
                        break;
                }
            }
        }

        public ObservableCollection<Book> booksList
        {
            get => _booksList;
            set => Set(ref _booksList, value);
        }


        #endregion

        #region commands

        public ICommand SwitchViewCommand { get; }
        private void OnSwitchViewCommandExecuted(object p)
        {
            parentVM.selectedVM = new SelectedBookViewModel(user, (int)p, parentVM);
        }

        #endregion


        public AllBooksViewModel(User user, MainWindowViewModel vm)
        {
            this.user = user;
            parentVM = vm;

            sortingSelected = sorting[0];

            SwitchViewCommand = new LambdaCommand(OnSwitchViewCommandExecuted);
        }
    }
}
