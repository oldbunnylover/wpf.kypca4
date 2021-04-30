using System.Windows.Controls;
using System.Windows.Input;

namespace kupca4.Views
{
    /// <summary>
    /// Логика взаимодействия для BookUpload.xaml
    /// </summary>
    public partial class AllBooks : UserControl
    {
        public AllBooks()
        {
            InitializeComponent();
        }

        private void Scroll(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scrollViewer = (ScrollViewer)sender;
            if (e.Delta < 0)
            {
                scrollViewer.LineDown();
            }
            else
            {
                scrollViewer.LineUp();
            }
            e.Handled = true;
        }
    }
}
