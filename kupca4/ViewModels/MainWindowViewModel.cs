using kupca4.DB;
using kupca4.ViewModels.Base;

namespace kupca4.ViewModels
{
    public class MainWindowViewModel : ViewModel
    {
        #region private fields

        private User user;
        private string _title;
        #endregion

        #region private fields

        public string title { get => user.Fullname; }

        #endregion

        public MainWindowViewModel()
        {

        }

        public MainWindowViewModel(User user)
        {
            this.user = user;
        }
    }
}
