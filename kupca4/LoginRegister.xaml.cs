using System.Windows;
using System.Windows.Input;

namespace kupca4
{
    public partial class LoginRegister : Window
    {
        public LoginRegister()
        {
            InitializeComponent();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
    }
}
