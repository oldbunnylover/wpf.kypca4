using kupca4.DB;
using System.Linq;
using kupca4.Helpers.Commands;
using kupca4.ViewModels.Base;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using System;
using Microsoft.Win32;
using System.Windows;
using System.Net;

namespace kupca4.ViewModels.Views
{
    public class BookUploadViewModel : ViewModel
    {
        #region private fields

        private readonly User user;
        private readonly KP_LibraryContext context = new KP_LibraryContext();
        private readonly BitmapImage _noPhoto = new BitmapImage(new Uri("pack://application:,,,/Styles/img/noPhoto.png"));
        private readonly Book editBook;
        private readonly WebClient myWebClient = new WebClient();
        private readonly MainWindowViewModel MainVM;

        private string _newGenre;
        private bool _dialog = false;
        private string _dialogText;
        private ObservableCollection<Genre> _genres = new ObservableCollection<Genre>((new KP_LibraryContext()).Genres);
        private Genre _selectedGenre;
        private string _title;
        private string _description;
        private BitmapImage _bookPicture = new BitmapImage();
        private BitmapImage _updatePicture = new BitmapImage();
        private string _imgPath;
        private string _pdfPath;
        private Visibility _fileCheck = Visibility.Collapsed;

        #endregion

        #region public fields

        public string newGenre
        {
            get => _newGenre;
            set => Set(ref _newGenre, value);
        }

        public ObservableCollection<Genre> genres
        {
            get => _genres;
            set => Set(ref _genres, value);
        }

        public bool dialog
        {
            get => _dialog;
            set => Set(ref _dialog, value);
        }

        public string dialogText
        {
            get => _dialogText;
            set => Set(ref _dialogText, value);
        }

        public Genre selectedGenre
        {
            get => _selectedGenre;
            set => Set(ref _selectedGenre, value);
        }

        public string title
        {
            get => _title;
            set => Set(ref _title, value);
        }

        public string description
        {
            get => _description;
            set => Set(ref _description, value);
        }

        public BitmapImage bookPicture
        {
            get => _bookPicture;
            set => Set(ref _bookPicture, value);
        }

        public Visibility fileCheck
        {
            get => _fileCheck;
            set => Set(ref _fileCheck, value);
        }

        #endregion

        private void RestoreForm()
        {
            if(editBook != null)
            {
                MainVM.selectedVM = new MyBooksViewModel(user, MainVM);
            }
            genres = new ObservableCollection<Genre>(context.Genres);
            title = "";
            description = "";
            bookPicture = _noPhoto;
            fileCheck = Visibility.Collapsed;
            _imgPath = "";
            _pdfPath = "";
        }

        #region commands

        public ICommand GenreAddCommand { get; }
        private bool CanGenreAddCommandExecute(object p) => newGenre?.Length > 0;
        private void OnGenreAddCommandExecuted(object p)
        {
            if (context.Genres.FirstOrDefault(g => g.Genrename == newGenre) == null)
            {
                context.Genres.Add(new Genre { Genrename = newGenre });
                context.SaveChanges();
                newGenre = "";
                genres = new ObservableCollection<Genre>(context.Genres);
            }
            else
            {
                dialogText = "Такой жанр уже есть.";
                dialog = true;
            }
        }

        public ICommand CloseDialogCommand { get; }
        private void OnCloseDialogCommandExecuted(object p) => dialog = false;

        public ICommand BookUploadCommand { get; }
        private bool CanBookUploadCommandExecute(object p) => true;
        private void OnBookUploadCommandExecuted(object p)
        {
            var book = new Book(title, description, selectedGenre.GenreId, user.Username);
            if (user.Role == 0)
                context.Users.Find(user.Username).Role = UserRole.Author;
            if (editBook == null)
            {
                context.Books.Add(book);
            }
            else
            {
                book.BookId = editBook.BookId;
                editBook.Bookname = title;
                editBook.Description = description;
                editBook.GenreId = selectedGenre.GenreId;
                editBook.Applied = BookStatus.NeedModer;
            }
            context.SaveChanges();
            try
            {
                byte[] responseArray = myWebClient.UploadFile($"http://localhost:3000/upload/{book.BookId}", _imgPath);
                MessageBox.Show(System.Text.Encoding.ASCII.GetString(responseArray));
                _updatePicture.BeginInit();
                _updatePicture.CacheOption = BitmapCacheOption.OnLoad;
                _updatePicture.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                _updatePicture.UriSource = new Uri($"http://localhost:3000/books/{book.BookId}/cover.png");
                _updatePicture.EndInit();
            }
            catch {
            }
            try
            {
                myWebClient.UploadFile($"http://localhost:3000/upload/{book.BookId}", _pdfPath);
            }
            catch { }
            RestoreForm();
        }

        public ICommand SelectImagePathCommand { get; }
        private void OnSelectImagePathCommandExecuted(object p)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Фотографии|*.jpg;*.png;*.jpeg;";
            if (openFileDialog.ShowDialog() == true)
            {
                bookPicture = new BitmapImage(new Uri(openFileDialog.FileName, UriKind.Absolute));
                _imgPath = openFileDialog.FileName;
            }
        }

        public ICommand SelectPdfPathCommand { get; }
        private void OnSelectPdfPathCommandExecuted(object p)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Файл книги|*.pdf";
            if (openFileDialog.ShowDialog() == true)
            {
                _pdfPath = openFileDialog.FileName;
                fileCheck = Visibility.Visible;
            }
        }

        #endregion

        public BookUploadViewModel(User user, MainWindowViewModel vm, int bookID = -1)
        {
            this.user = user;
            MainVM = vm;
            if (bookID != -1)
            {
                editBook = context.Books.Find(bookID);
                fileCheck = Visibility.Visible;
                title = editBook.Bookname;
                description = editBook.Description;
                try
                {
                    _bookPicture.BeginInit();
                    _bookPicture.CacheOption = BitmapCacheOption.OnLoad;
                    _bookPicture.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                    _bookPicture.UriSource = new Uri($"http://localhost:3000/books/{bookID}/cover.png");
                    _bookPicture.EndInit();
                }
                catch
                {
                    bookPicture = _noPhoto;
                }
            }
            else
            {
                bookPicture = _noPhoto;
            }

            GenreAddCommand = new LambdaCommand(OnGenreAddCommandExecuted, CanGenreAddCommandExecute);
            CloseDialogCommand = new LambdaCommand(OnCloseDialogCommandExecuted);
            BookUploadCommand = new LambdaCommand(OnBookUploadCommandExecuted, CanBookUploadCommandExecute);
            SelectImagePathCommand = new LambdaCommand(OnSelectImagePathCommandExecuted);
            SelectPdfPathCommand = new LambdaCommand(OnSelectPdfPathCommandExecuted);
        }
    }
}
