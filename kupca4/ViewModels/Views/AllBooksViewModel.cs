using kupca4.DB;
using kupca4.ViewModels.Base;

namespace kupca4.ViewModels.Views
{
    class AllBooksViewModel : ViewModel
    {
        private readonly User user;
        private readonly KP_LibraryContext context = new KP_LibraryContext();


        public AllBooksViewModel(User user)
        {
            this.user = user;
        }
    }
}
