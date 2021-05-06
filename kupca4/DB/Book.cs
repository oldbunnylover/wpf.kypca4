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
        public bool? Applyed { get; set; }
        public bool? Hidden { get; set; }
        public int GenreId { get; set; }
        public string AuthorName { get; set; }

        public Book(string Bookname, string Description, int GenreId, string AuthorName)
        {
            this.Bookname = Bookname;
            this.Description = Description;
            this.GenreId = GenreId;
            this.AuthorName = AuthorName;
        }

        public virtual Genre Genre { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<SavedBook> SavedBooks { get; set; }
        public virtual ICollection<BookRates> BookRates { get; set; }
    }
}
