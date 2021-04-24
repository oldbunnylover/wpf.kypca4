using System.Windows;

namespace kupca4.Helpers.Commands
{
    class AppClose : Command
    {
        public override bool CanExecute(object parameter) => true;

        public override void Execute(object parameter) => Application.Current.Shutdown();
    }
}
