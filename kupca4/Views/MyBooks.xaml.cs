using System.Windows.Controls;
using System.Windows.Input;

namespace kupca4.Views
{
    /// <summary>
    /// Логика взаимодействия для BookUpload.xaml
    /// </summary>
    public partial class MyBooks : UserControl
    {
        public MyBooks()
        {
            InitializeComponent();
        }

        private void Scroll(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scrollViewer = (ScrollViewer)sender;
            if (e.Delta < 0)
            {
                scrollViewer.LineRight();
            }
            else
            {
                scrollViewer.LineLeft();
            }
            e.Handled = true;
        }
    }
}
