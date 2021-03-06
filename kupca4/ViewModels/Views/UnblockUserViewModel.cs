using kupca4.DB;
using kupca4.Helpers.Commands;
using kupca4.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace kupca4.ViewModels.Views
{
    class UnblockUserViewModel : ViewModel
    {
        #region private fields

        private readonly MainWindowViewModel MainVM;
        private readonly UserViewModel parentVM;
        private readonly User user;
        private readonly KP_LibraryContext context = new KP_LibraryContext();

        private ObservableCollection<User> _blockedUsersList;

        #endregion

        #region public fields

        public ObservableCollection<User> blockedUsersList
        {
            get => _blockedUsersList;
            set => Set(ref _blockedUsersList, value);
        }

        #endregion

        #region commands

        public ICommand UnblockUserCommand { get; }
        private void OnUnblockUserCommandExecuted(object p)
        {
            try
            {
                context.Users.Find((string)p).Blocked = false;
                context.Books.First(b => b.AuthorName == (string)p && b.Applied == BookStatus.Banned).Applied = BookStatus.Canceled;
                context.SaveChanges();
                blockedUsersList = new ObservableCollection<User>(context.Users.Where(u => u.Blocked == true));
            }
            catch
            {
                parentVM.noEthernetDialog = true;
                parentVM.noEthernetDialogText = "Отсутствует подключение к интернету.";
            }
        }

        public ICommand BlockReasonInfoCommand { get; }
        private void OnBlockReasonInfoCommandExecuted(object p)
        {
            try
            {
                MainVM.selectedVM = new SelectedBookViewModel(user, context.Books.First(b => b.AuthorName == (string)p && b.Applied == BookStatus.Banned).BookId,
                MainVM, new UserViewModel(user, MainVM, this));
            }
            catch
            {
                parentVM.noEthernetDialog = true;
                parentVM.noEthernetDialogText = "Отсутствует подключение к интернету.";
            }
        }

        #endregion

        public UnblockUserViewModel(MainWindowViewModel MainVM, User user, UserViewModel parentVM)
        {
            this.user = user;
            this.MainVM = MainVM;
            this.parentVM = parentVM;

            _blockedUsersList = new ObservableCollection<User>(context.Users.Where(u => u.Blocked == true));
            UnblockUserCommand = new LambdaCommand(OnUnblockUserCommandExecuted);
            BlockReasonInfoCommand = new LambdaCommand(OnBlockReasonInfoCommandExecuted);
        }
    }
}
