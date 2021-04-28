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

namespace kupca4.ViewModels.Views
{
    public class BookUploadViewModel : ViewModel
    {
        #region private fields

        private readonly User user;
        private readonly KP_LibraryContext context = new KP_LibraryContext();
        private string _newGenre;
        private bool _dialog = false;
        private string _dialogText;
        private ObservableCollection<Genre> _genres = new ObservableCollection<Genre>((new KP_LibraryContext()).Genres);
        private Genre _selectedGenre;
        private string _title;
        private string _description;
        private BitmapImage _bookPicture = new BitmapImage(new Uri("pack://application:,,,/Styles/img/noPhoto.png"));
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


        #region commands

        public ICommand GenreAddCommand { get; }
        private bool CanGenreAddCommandExecute(object p) => true;
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
        private bool CanCloseDialogCommandExecute(object p) => true;
        private void OnCloseDialogCommandExecuted(object p) => dialog = false;

        public ICommand BookUploadCommand { get; }
        private bool CanBookUploadCommandExecute(object p) => true;
        private void OnBookUploadCommandExecuted(object p)
        {
            var book = new Book(title, description, selectedGenre.GenreId, user.Username);
            context.Books.Add(book);
            context.SaveChanges();
        }

        public ICommand SelectImagePathCommand { get; }
        private bool CanSelectImagePathCommandExecute(object p) => true;
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
        private bool CanSelectPdfPathCommandExecute(object p) => true;
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


        public BookUploadViewModel(User user)
        {
            this.user = user;
            GenreAddCommand = new LambdaCommand(OnGenreAddCommandExecuted, CanGenreAddCommandExecute);
            CloseDialogCommand = new LambdaCommand(OnCloseDialogCommandExecuted, CanCloseDialogCommandExecute);
            BookUploadCommand = new LambdaCommand(OnBookUploadCommandExecuted, CanBookUploadCommandExecute);
            SelectImagePathCommand = new LambdaCommand(OnSelectImagePathCommandExecuted, CanSelectImagePathCommandExecute);
            SelectPdfPathCommand = new LambdaCommand(OnSelectPdfPathCommandExecuted, CanSelectPdfPathCommandExecute);
        }
    }
}
