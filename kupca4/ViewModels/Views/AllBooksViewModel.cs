using kupca4.DB;
using kupca4.Helpers.Commands;
using kupca4.ViewModels.Base;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace kupca4.ViewModels.Views
{
    class AllBooksViewModel : ViewModel
    {
        #region private fields

        private readonly User user;
        private readonly KP_LibraryContext context = new KP_LibraryContext();
        private readonly List<string> _sorting = new List<string>{ "по новизне", "по алфавиту" };
        private readonly MainWindowViewModel parentVM;

        private string _searchString;
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
                try
                {
                    Set(ref _sortingSelected, value);
                    switch (value)
                    {
                        case "по новизне":
                            booksList = new ObservableCollection<Book>(context.Books.OrderByDescending(b => b.BookId).Where(b => b.Hidden == false && b.Applied == BookStatus.Applied));
                            break;
                        case "по алфавиту":
                            booksList = new ObservableCollection<Book>(context.Books.OrderBy(b => b.Bookname).Where(b => b.Hidden == false && b.Applied == BookStatus.Applied));
                            break;
                    }
                }
                catch
                {
                    parentVM.dialog = true;
                    parentVM.dialogText = "Отсутствует подключение к интернету.";
                }
            }
        }

        public ObservableCollection<Book> booksList
        {
            get => _booksList;
            set => Set(ref _booksList, value);
        }

        public string searchString
        {
            get => _searchString;
            set
            {
                try
                {
                    Set(ref _searchString, value);
                    if (value.Length == 0)
                        sortingSelected = sortingSelected;
                    else
                        booksList = new ObservableCollection<Book>(context.Books.Where(b => b.Bookname.StartsWith(value) && b.Applied == BookStatus.Applied));
                }
                catch
                {
                    parentVM.dialog = true;
                    parentVM.dialogText = "Отсутствует подключение к интернету.";
                }
            }
        }

        #endregion

        #region commands

        public ICommand SwitchViewCommand { get; }
        private void OnSwitchViewCommandExecuted(object p)
        {
            try
            {
                parentVM.selectedVM = new SelectedBookViewModel(user, (int)p, parentVM, this);
            }
            catch
            {
                parentVM.dialog = true;
                parentVM.dialogText = "Отсутствует подключение к интернету.";
            }
        }

        #endregion


        public AllBooksViewModel(User user, MainWindowViewModel vm)
        {
            this.user = user;
            parentVM = vm;

            sortingSelected = "по новизне";

            SwitchViewCommand = new LambdaCommand(OnSwitchViewCommandExecuted);
        }
    }
}
