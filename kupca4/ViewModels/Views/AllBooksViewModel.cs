using kupca4.DB;
using kupca4.Helpers.Commands;
using kupca4.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace kupca4.ViewModels.Views
{
    class AllBooksViewModel : ViewModel
    {
        #region private fields

        private readonly User user;
        private readonly List<string> _sorting = new List<string>{ "по новизне", "по алфавиту", "по популярности" };
        private readonly MainWindowViewModel parentVM;

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

        public ICommand SwitchViewCommand { get; }
        private bool CanSwitchViewCommandExecute(object p) => true;
        private void OnSwitchViewCommandExecuted(object p)
        {
            parentVM.selectedVM = new SelectedBookViewModel(user, (int)p, parentVM);
        }

        #endregion


        public AllBooksViewModel(User user, MainWindowViewModel vm)
        {
            this.user = user;
            parentVM = vm;

            _sortingSelected = _sorting[0];

            SwitchViewCommand = new LambdaCommand(OnSwitchViewCommandExecuted, CanSwitchViewCommandExecute);
        }
    }
}
