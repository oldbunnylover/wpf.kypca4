using System.Windows.Controls;

namespace kupca4.Views
{
    /// <summary>
    /// Логика взаимодействия для User.xaml
    /// </summary>
    public partial class User : UserControl
    {
        public User()
        {
            InitializeComponent();
        }

        private void ComboBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (((ComboBox)sender).Text.Length == 1)
            {
                var tb = (TextBox)e.OriginalSource;
                tb.Select(tb.SelectionStart + tb.SelectionLength, 0);
            }
        }
    }
}
