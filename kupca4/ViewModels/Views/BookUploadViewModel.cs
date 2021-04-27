using kupca4.DB;
using kupca4.ViewModels.Base;

namespace kupca4.ViewModels.Views
{
    class BookUploadViewModel : ViewModel
    {
        private readonly User user;

        public string test
        {
            get => user.Fullname;
        }

        public BookUploadViewModel(User user)
        {
            this.user = user;
        }
    }
}
