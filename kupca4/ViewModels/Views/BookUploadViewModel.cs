using kupca4.DB;
using System.Linq;
using kupca4.Helpers.Commands;
using kupca4.ViewModels.Base;
using System.Windows.Input;
using System.Collections.Generic;

namespace kupca4.ViewModels.Views
{
    public class BookUploadViewModel : ViewModel
    {
        #region private fields

        private readonly User user;
        private readonly KP_LibraryContext context = new KP_LibraryContext();
        private string _newGenre;
        private List<Genre> _genres;

        #endregion

        #region public fields

        public string newGenre
        {
            get => _newGenre;
            set => Set(ref _newGenre, value);
        }

        public List<Genre> genres
        {
            get => _genres;
            set => Set(ref _genres, value);
        }

        #endregion

        public List<Genre> GetGenreList()
        {
            return (from genre in context.Genres select genre).ToList();
        }

        #region commands

        public ICommand GenreAdd { get; }
        private bool CanGenreAddExecute(object p) => newGenre?.Length > 0;
        private void OnGenreAddExecuted(object p)
        {
            context.Genres.Add(new Genre { Genrename = newGenre });
            context.SaveChanges();
        }

        #endregion


        public BookUploadViewModel(User user)
        {
            this.user = user;
            GenreAdd = new LambdaCommand(OnGenreAddExecuted, CanGenreAddExecute);
        }
    }
}
