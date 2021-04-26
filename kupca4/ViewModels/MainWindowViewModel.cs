﻿using kupca4.DB;
using kupca4.Helpers.Commands;
using kupca4.ViewModels.Base;
using System.Windows;
using System.Windows.Input;

namespace kupca4.ViewModels
{
    public class MainWindowViewModel : ViewModel
    {
        #region private fields

        private User user;
        private WindowState _windowState = WindowState.Normal;
        #endregion

        #region public fields

        public WindowState windowState
        {
            get => _windowState;
            set => Set(ref _windowState, value);
        }

        public string test
        {
            get => user.Fullname;
        }
        #endregion

        #region Commands

        public ICommand WindowMinimizedCommand { get; }
        private bool CanWindowMinimizedCommandExecute(object p) => true;
        private void OnWindowMinimizedCommandExecuted(object p) => windowState = WindowState.Minimized;

        public ICommand WindowMaximizeCommand { get; }
        private bool CanWindowMaximizeCommandExecute(object p) => true;
        private void OnWindowMaximizeCommandExecuted(object p)
        {
            windowState = windowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        public ICommand SwitchUserCommand { get; }
        private bool CanSwitchUserCommandExecute(object p) => true;
        private void OnSwitchUserCommandExecuted(object p)
        {
            var LoginRegister = new LoginRegister();
            LoginRegister.Show();
            Application.Current.MainWindow.Close();
        }

        #endregion

        public MainWindowViewModel(User user)
        {
            this.user = user;

            WindowMinimizedCommand = new LambdaCommand(OnWindowMinimizedCommandExecuted, CanWindowMinimizedCommandExecute);
            WindowMaximizeCommand = new LambdaCommand(OnWindowMaximizeCommandExecuted, CanWindowMaximizeCommandExecute);
            SwitchUserCommand = new LambdaCommand(OnSwitchUserCommandExecuted, CanSwitchUserCommandExecute);
        }
    }
}
