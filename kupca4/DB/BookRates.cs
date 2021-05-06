#nullable disable

namespace kupca4.DB
{
    public partial class BookRates
    {
        public string Username { get; set; }
        public int BookId { get; set; }
        public int? UserRate { get; set; }

        public virtual Book Book { get; set; }
        public virtual User UsernameNavigation { get; set; }
    }
}
