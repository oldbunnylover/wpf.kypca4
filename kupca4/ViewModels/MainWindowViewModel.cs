﻿using kupca4.DB;
using kupca4.Helpers.Commands;
using kupca4.ViewModels.Base;
using kupca4.ViewModels.Views;
using System.Windows;
using System.Windows.Input;

namespace kupca4.ViewModels
{
    public class MainWindowViewModel : ViewModel
    {
        #region private fields

        private readonly User user;
        private ViewModel _selectedVM;
        private WindowState _windowState = WindowState.Normal;
        private bool _uploadBookMenuItemVisability;
        private bool _contrloPaneMenuItemVisability;
        #endregion

        #region public fields

        public ViewModel selectedVM
        {
            get => _selectedVM;
            set => Set(ref _selectedVM, value);
        }

        public WindowState windowState
        {
            get => _windowState;
            set => Set(ref _windowState, value);
        }

        public bool uploadBookMenuItemVisability
        {
            get => _uploadBookMenuItemVisability;
        }

        public bool contrloPaneMenuItemVisability
        {
            get => _contrloPaneMenuItemVisability;
        }
        #endregion

        #region Commands

        public ICommand WindowMinimizedCommand { get; }
        private void OnWindowMinimizedCommandExecuted(object p) => windowState = WindowState.Minimized;

        public ICommand WindowMaximizeCommand { get; }
        private void OnWindowMaximizeCommandExecuted(object p)
        {
            windowState = windowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        public ICommand SwitchUserCommand { get; }
        private void OnSwitchUserCommandExecuted(object p)
        {
            var LoginRegister = new LoginRegister();
            LoginRegister.Show();
            (p as Window).Close();
        }

        public ICommand SwitchViewCommand { get; }
        private void OnSwitchViewCommandExecuted(object p)
        {
            switch (p.ToString())
            {
                case "BookUpload":
                    selectedVM = new BookUploadViewModel(user, this);
                    break;
                case "AllBooks":
                    selectedVM = new AllBooksViewModel(user, this);
                    break;
                case "MyBooks":
                    selectedVM = new MyBooksViewModel(user, this);
                    break;
                case "User":
                    selectedVM = new UserViewModel(user, this);
                    break;
            }
        }

        #endregion

        public MainWindowViewModel(User user, string view)
        {
            this.user = user;

            _uploadBookMenuItemVisability = user.Role != UserRole.Moderator;
            _contrloPaneMenuItemVisability = user.Role != UserRole.User && user.Role != UserRole.Author;

            WindowMinimizedCommand = new LambdaCommand(OnWindowMinimizedCommandExecuted);
            WindowMaximizeCommand = new LambdaCommand(OnWindowMaximizeCommandExecuted);
            SwitchUserCommand = new LambdaCommand(OnSwitchUserCommandExecuted);
            SwitchViewCommand = new LambdaCommand(OnSwitchViewCommandExecuted);

            if (view == "MyBooks")
                selectedVM = new MyBooksViewModel(user, this);
            else
                selectedVM = new AllBooksViewModel(user, this);
        }
    }
}
