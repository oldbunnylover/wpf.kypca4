using System.Collections.Generic;

#nullable disable

namespace kupca4.DB
{
    public partial class User
    {
        public User()
        {
            BooksAuthors = new HashSet<BooksAuthor>();
            SavedBooks = new HashSet<SavedBook>();
        }

        public string Username { get; set; }
        public int? Role { get; set; }
        public string Fullname { get; set; }
        public string Password { get; set; }
        public bool? Blocked { get; set; }

        public virtual ICollection<BooksAuthor> BooksAuthors { get; set; }
        public virtual ICollection<SavedBook> SavedBooks { get; set; }
    }
}
