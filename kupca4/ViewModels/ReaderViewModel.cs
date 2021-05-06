using kupca4.DB;
using kupca4.ViewModels.Base;

namespace kupca4.ViewModels
{
    class ReaderViewModel : ViewModel
    {
        private readonly string _bookPath;
        private readonly string _title;

        public string bookPath
        {
            get => _bookPath;
        }

        public string title
        {
            get => _title;
        }

        public ReaderViewModel(Book book)
        {
            _bookPath = $"http://localhost:3000/books/{book.BookId}/book.pdf";
            _title = $"{book.Bookname} - {book.AuthorName}";
        }
    }
}
