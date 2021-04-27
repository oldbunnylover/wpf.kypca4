using System.Collections.Generic;

#nullable disable

namespace kupca4.DB
{
    public partial class Book
    {
        public Book()
        {
            SavedBooks = new HashSet<SavedBook>();
        }

        public int BookId { get; set; }
        public string Bookname { get; set; }
        public string Description { get; set; }
        public string Filepath { get; set; }
        public bool? Applyed { get; set; }
        public int? GenreId { get; set; }

        public virtual Genre Genre { get; set; }
        public virtual ICollection<SavedBook> SavedBooks { get; set; }
    }
}
