using System.Collections.Generic;

#nullable disable

namespace kupca4.DB
{
    public partial class Genre
    {
        public Genre()
        {
            Books = new HashSet<Book>();
        }

        public int GenreId { get; set; }
        public string Genrename { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }
}
