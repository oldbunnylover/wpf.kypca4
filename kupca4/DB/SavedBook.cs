#nullable disable

namespace kupca4.DB
{
    public partial class SavedBook
    {
        public string Username { get; set; }
        public int BookId { get; set; }

        public virtual Book Book { get; set; }
        public virtual User UsernameNavigation { get; set; }
    }
}
