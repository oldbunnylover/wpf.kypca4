using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

#nullable disable

namespace kupca4.DB
{
    public partial class User
    {
        public User()
        {
            SavedBooks = new HashSet<SavedBook>();
        }

        public string Username { get; set; }
        public int? Role { get; set; }
        public string Fullname { get; set; }
        public string Password { get; set; }
        public bool? Blocked { get; set; }

        public User(string name, string surname, string username, string password)
        {
            this.Username = username;
            this.Fullname = name + " " + surname;
            this.Password = getHash(password);
        }

        public virtual ICollection<SavedBook> SavedBooks { get; set; }
        public virtual ICollection<BookRates> BookRates { get; set; }
        public virtual ICollection<Book> Books { get; set; }

        public static string getHash(string password)
        {
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hash);
        }
    }
}
