﻿using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using kupca4.DB;
using kupca4.Helpers.Commands;
using kupca4.ViewModels.Base;

namespace kupca4.ViewModels
{
    class LoginRegisterViewModel : ViewModel
    {
        #region private fields

        private KP_LibraryContext context = new KP_LibraryContext();
        private string _name;
        private string _surname;
        private string _registerUsername;
        private bool _loading = false;
        private bool _dialog = false;
        private string _dialogText;

        #endregion

        #region public fields

        public string name
        {
            get => _name;
            set => Set(ref _name, value);
        }

        public string surname
        {
            get => _surname;
            set => Set(ref _surname, value);
        }

        public string registerUsername
        {
            get => _registerUsername;
            set => Set(ref _registerUsername, value);
        }

        public bool loading
        {
            get => _loading;
            set => Set(ref _loading, value);
        }

        public bool dialog
        {
            get => _dialog;
            set => Set(ref _dialog, value);
        }

        public string dialogText
        {
            get => _dialogText;
            set => Set(ref _dialogText, value);
        }

        #endregion

        #region Commands

        public ICommand RegisterCommand { get; }
        private bool CanRegisterCommandExecute(object p) => name?.Length > 0 && surname?.Length > 0 
            && registerUsername?.Length > 0 && (p as PasswordBox).Password?.Length > 0;

        private void OnRegisterCommandExecuted(object p)
        {
            var password = (p as PasswordBox).Password;
            ThreadPool.QueueUserWorkItem(
                delegate
                {
                    if (context.Users.FirstOrDefault(u => u.Username == registerUsername) == null)
                    {
                        User user = new User(name, surname, registerUsername, password);
                        context.Users.Add(user);
                        context.SaveChanges();
                        var MainWindowViewModel = new MainWindowViewModel(user);
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            var MainWindow = new MainWindow
                            {
                                DataContext = MainWindowViewModel
                            };
                            MainWindow.Show();
                            Application.Current.MainWindow.Close();
                        });
                    }
                    else
                    {
                        dialog = true;
                        dialogText = "Пользователь с данным псевдонимом уже зарегистрирован.";
                    }
                }
            );
        }

        public ICommand CloseDialogCommand { get; }
        private bool CanCloseDialogCommandExecute(object p) => true;
        private void OnCloseDialogCommandExecuted(object p) => dialog = false;

        #endregion

        public LoginRegisterViewModel()
        {
            #region Commands

            RegisterCommand = new LambdaCommand(OnRegisterCommandExecuted, CanRegisterCommandExecute);
            CloseDialogCommand = new LambdaCommand(OnCloseDialogCommandExecuted, CanCloseDialogCommandExecute);

            #endregion
        }
    }
}
