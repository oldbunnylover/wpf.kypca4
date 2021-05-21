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
using System.Text.RegularExpressions;

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
        private ObservableCollection<string> _genres;
        private string _selectedGenreName;
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

        public ObservableCollection<string> genres
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

        public string selectedGenreName
        {
            get => _selectedGenreName;
            set => Set(ref _selectedGenreName, value);
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
            selectedGenreName = null;
            title = "";
            description = "";
            bookPicture = _noPhoto;
            fileCheck = Visibility.Collapsed;
            _imgPath = "";
            _pdfPath = "";
        }

        public void GenreInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^а-яА-яa-zA-Z]$").IsMatch(e.Text);
        }

        #region commands

        public ICommand GenreAddCommand { get; }
        private bool CanGenreAddCommandExecute(object p) => newGenre?.Length > 1 && newGenre?.Length < 41;
        private void OnGenreAddCommandExecuted(object p)
        {
            try
            {
                if (context.Genres.FirstOrDefault(g => g.Genrename == newGenre) == null)
                {
                    context.Genres.Add(new Genre { Genrename = newGenre });
                    context.SaveChanges();
                    newGenre = "";
                    genres = new ObservableCollection<string>(context.Genres.Select(g => g.Genrename));
                }
                else
                {
                    dialogText = "Такой жанр уже есть.";
                    dialog = true;
                }
            }
            catch
            {
                MainVM.dialog = true;
                MainVM.dialogText = "Отсутствует подключение к интернету.";
            }
        }

        public ICommand CloseDialogCommand { get; }
        private void OnCloseDialogCommandExecuted(object p) => dialog = false;

        public ICommand BookUploadCommand { get; }
        private bool CanBookUploadCommandExecute(object p) => title?.Length > 1 && title?.Length < 65 && description?.Length > 1 && selectedGenreName?.Length > 0
            && description?.Length < 1851 && (editBook != null || _imgPath?.Length > 0) && (editBook != null || _pdfPath?.Length > 0);
        private void OnBookUploadCommandExecuted(object p)
        {
            try
            {
                var book = new Book(title, description, context.Genres.FirstOrDefault(g => g.Genrename == selectedGenreName).GenreId, user.Username);
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
                    editBook.GenreId = context.Genres.FirstOrDefault(g => g.Genrename == selectedGenreName).GenreId;
                    editBook.Applied = editBook.Applied == BookStatus.Banned ? BookStatus.Banned : BookStatus.NeedModer;
                }
                context.SaveChanges();
                try
                {
                    myWebClient.UploadFile($"http://localhost:3000/upload/{book.BookId}", _imgPath);
                    _updatePicture.BeginInit();
                    _updatePicture.CacheOption = BitmapCacheOption.OnLoad;
                    _updatePicture.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                    _updatePicture.UriSource = new Uri($"http://localhost:3000/books/{book.BookId}/cover.png");
                    _updatePicture.EndInit();
                }
                catch { }
                try
                {
                    myWebClient.UploadFile($"http://localhost:3000/upload/{book.BookId}", _pdfPath);
                }
                catch { }

                dialog = true;
                dialogText = "Загрузка выполнена успешно!";
                RestoreForm();
            }
            catch
            {
                MainVM.dialog = true;
                MainVM.dialogText = "Отсутствует подключение к интернету.";
            }
        }

        public ICommand SelectImagePathCommand { get; }
        private void OnSelectImagePathCommandExecuted(object p)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Фотографии|*.jpg;*.png;*.jpeg;";
                if (openFileDialog.ShowDialog() == true)
                {
                    bookPicture = new BitmapImage(new Uri(openFileDialog.FileName, UriKind.Absolute));
                    _imgPath = openFileDialog.FileName;
                }
            }
            catch
            {
                dialog = true;
                dialogText = "Некорректный формат изображения.";
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

            _genres = new ObservableCollection<string>(context.Genres.Select(g => g.Genrename));

            if (bookID != -1)
            {
                editBook = context.Books.Find(bookID);
                fileCheck = Visibility.Visible;
                title = editBook.Bookname;
                description = editBook.Description;
                selectedGenreName = context.Genres.Find(editBook.GenreId).Genrename;
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
