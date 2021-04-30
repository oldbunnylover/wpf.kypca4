using kupca4.DB;
using kupca4.ViewModels.Base;

namespace kupca4.ViewModels.Views
{
    class MyBooksViewModel : ViewModel
    {
        private readonly User user;
        private readonly KP_LibraryContext context = new KP_LibraryContext();

        public MyBooksViewModel(User user)
        {
            this.user = user;
        }
    }
}
