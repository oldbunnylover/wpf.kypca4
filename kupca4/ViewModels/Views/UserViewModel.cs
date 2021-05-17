using kupca4.DB;
using kupca4.Helpers.Commands;
using kupca4.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace kupca4.ViewModels.Views
{
    class UserViewModel : ViewModel
    {
        #region private fields

        private readonly MainWindowViewModel MainVM;
        private readonly KP_LibraryContext context = new KP_LibraryContext();

        private readonly User user;
        private ViewModel _selectedVM;
        private bool _dialog = false;
        private bool _noEthernetDialog = false;
        private string _noEthernetDialogText;
        private bool _searchResults = false;
        private bool _isAdmin;
        private User _selectedModer;
        private ObservableCollection<User> _moderList;
        private string _searchString;

        #endregion

        #region public fields

        public ViewModel selectedVM
        {
            get => _selectedVM;
            set => Set(ref _selectedVM, value);
        }

        public bool dialog
        {
            get => _dialog;
            set => Set(ref _dialog, value);
        }

        public bool noEthernetDialog
        {
            get => _noEthernetDialog;
            set => Set(ref _noEthernetDialog, value);
        }

        public string noEthernetDialogText
        {
            get => _noEthernetDialogText;
            set => Set(ref _noEthernetDialogText, value);
        }

        public bool isAdmin
        {
            get => _isAdmin;
        }

        public bool searchResults
        {
            get => _searchResults;
            set => Set(ref _searchResults, value);
        }

        public string searchString
        {
            get => _searchString;
            set
            {
                try
                {
                    Set(ref _searchString, value);
                    searchResults = true;
                    moderList = new ObservableCollection<User>(context.Users.Where(u => u.Username.StartsWith(value) && u.Role != UserRole.Moderator && u.Role != UserRole.Admin).Take(5));
                }
                catch
                {
                    dialog = false;
                    noEthernetDialog = true;
                    noEthernetDialogText = "Отсутствует подключение к интернету.";
                }
            }
        }

        public User selectedModer
        {
            get => _selectedModer;
            set => Set(ref _selectedModer, value);
        }

        public ObservableCollection<User> moderList
        {
            get => _moderList;
            set => Set(ref _moderList, value);
        }

        #endregion

        #region commands

        public ICommand SwitchViewCommand { get; }
        private void OnSwitchViewCommandExecuted(object p)
        {
            try
            {
                context.Users.FirstOrDefault(u => u.Username == user.Username);
                switch (p.ToString())
                {
                    case "BooksApply":
                        selectedVM = new BooksApplyViewModel(MainVM, user, this);
                        break;
                    case "AuthorUnblock":
                        selectedVM = new UnblockUserViewModel(MainVM, user, this);
                        break;
                    case "NewModerator":
                        dialog = true;
                        break;
                }
            }
            catch
            {
                noEthernetDialog = true;
                noEthernetDialogText = "Отсутствует подключение к интернету.";
            }
        }

        public ICommand CloseDialogCommand { get; }
        private void OnCloseDialogCommandExecuted(object p)
        {
            dialog = false;
        }

        public ICommand NewModeratorCommand { get; }
        private bool CanNewModeratorCommandExecute(object p) => selectedModer != null;
        private void OnNewModeratorCommandExecuted(object p)
        {
            try
            {
                selectedModer.Role = UserRole.Moderator;
                context.SaveChanges();
                dialog = false;
                searchString = "";
            }
            catch
            {
                noEthernetDialog = true;
                noEthernetDialogText = "Отсутствует подключение к интернету.";
            }
        }

        public ICommand CloseNoEthernetDialogCommand { get; }
        private void OnCloseNoEthernetDialogCommandExecuted(object p) => noEthernetDialog = false;

        #endregion

        public UserViewModel(User user, MainWindowViewModel MainVM, ViewModel selectedVM = null)
        {
            this.user = user;
            this.MainVM = MainVM;
            _selectedVM = selectedVM;
            _isAdmin = user.Role == UserRole.Admin;

            SwitchViewCommand = new LambdaCommand(OnSwitchViewCommandExecuted);
            CloseDialogCommand = new LambdaCommand(OnCloseDialogCommandExecuted);
            CloseNoEthernetDialogCommand = new LambdaCommand(OnCloseNoEthernetDialogCommandExecuted);
            NewModeratorCommand = new LambdaCommand(OnNewModeratorCommandExecuted, CanNewModeratorCommandExecute);
        }
    }
}
