using kupca4.DB;
using kupca4.Helpers.Commands;
using kupca4.ViewModels.Base;
using kupca4.ViewModels.Views;
using MaterialDesignThemes.Wpf;
using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace kupca4.ViewModels
{
    public class MainWindowViewModel : ViewModel
    {
        #region private fields

        private User user;
        private readonly KP_LibraryContext context = new KP_LibraryContext();
        
        private SnackbarMessageQueue Queue = new SnackbarMessageQueue();
        private ViewModel _selectedVM;
        private WindowState _windowState = WindowState.Normal;
        private bool _uploadBookMenuItemVisability;
        private bool _contrloPaneMenuItemVisability;
        private bool _dialog = false;
        private bool _dialogSettings = false;
        private string _dialogText;
        private string _email;
        private string _newPassword;
        private string _repeatNewPassword;
        private string _passError;
        private bool _passErrorVisibility = false;
        private bool _confirmEmailButton;
        private bool _confirmCodeEmailVisability = false;
        private string _confirmCodeEmail;
        private bool _isUserEmailNull;
        private int code;

        #endregion

        #region public fields

        public string passError
        {
            get => _passError;
            set => Set(ref _passError, value);
        }

        public bool passErrorVisibility
        {
            get => _passErrorVisibility;
            set => Set(ref _passErrorVisibility, value);
        }
        
        public bool confirmEmailButton
        {
            get => _confirmEmailButton;
            set => Set(ref _confirmEmailButton, value);
        }

        public bool isUserEmailNull
        {
            get => _isUserEmailNull;
            set => Set(ref _isUserEmailNull, value);
        }

        public string confirmCodeEmail
        {
            get => _confirmCodeEmail;
            set => Set(ref _confirmCodeEmail, value);
        }

        public bool confirmCodeEmailVisability
        {
            get => _confirmCodeEmailVisability;
            set => Set(ref _confirmCodeEmailVisability, value);
        }

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

        public bool dialog
        {
            get => _dialog;
            set => Set(ref _dialog, value);
        }

        public bool dialogSettings
        {
            get => _dialogSettings;
            set => Set(ref _dialogSettings, value);
        }

        public string dialogText
        {
            get => _dialogText;
            set => Set(ref _dialogText, value);
        }

        public string email
        {
            get => _email;
            set
            {
                Set(ref _email, value);
                confirmEmailButton = true;
                confirmCodeEmailVisability = false;
            }
        }

        public string newPassword
        {
            get => _newPassword;
            set 
            {
                Set(ref _newPassword, value);
                passErrorVisibility = false;
                if (value != null && value.Length != 0)
                {
                    if (value.Length < 6)
                    {
                        passError = "Пароль должен состоять минимум из 6 символов.";
                        passErrorVisibility = true;
                    }
                    if (value.Length > 25)
                    {
                        passError = "Пароль должен состоять максимум из 25 символов.";
                        passErrorVisibility = true;
                    }
                    if (!new Regex("^[a-zA-Z0-9!?.@]+$").IsMatch(value))
                    {
                        passError = "Пароль может содержать только латиницу, цифры и '!', '?', '.', '@'";
                        passErrorVisibility = true;
                    }
                }
            }
        }

        public string repeatNewPassword
        {
            get => _repeatNewPassword;
            set
            {
                Set(ref _repeatNewPassword, value);
                if(passErrorVisibility && passError == "Пароли не совпадают.")
                    passErrorVisibility = false;
            }
        }

        public SnackbarMessageQueue queue
        {
            get => Queue;
            set => Set(ref Queue, value);
        }

        public bool menuSpace
        {
            get => user.Role == UserRole.Moderator || user.Blocked == true;
        }

        public User User
        {
            get => user;
        }

        #endregion

        #region mailSend

        private void SendEmail(string str, int code)
        {
            try
            {
                MailAddress from = new MailAddress("ducklibrarynoreply@gmail.com", "DuckLibrary");
                MailAddress to = new MailAddress(str);
                MailMessage m = new MailMessage(from, to);
                m.Subject = "Добро пожаловать!";
                m.Body = $"Здравствуйте! Чтобы подтвердить свой адрес электронной почты, введите этот код в приложении: \n{code}";
                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.Credentials = new NetworkCredential("ducklibrarynoreply@gmail.com", "NotReallyPassWord");
                smtp.EnableSsl = true;
                smtp.SendMailAsync(m);
            }
            catch
            {
                dialogText = "Отсутствует подключение к интернету.";
                dialog = true;
            }
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

        public ICommand CloseDialogCommand { get; }
        private void OnCloseDialogCommandExecuted(object p) => dialog = false;

        public ICommand ShowDialogSettingsCommand { get; }
        private void OnShowDialogSettingsCommandExecuted(object p) => dialogSettings = true;
        
        public ICommand CloseDialogSettingsCommand { get; }
        private void OnCloseDialogSettingsCommandExecuted(object p)
        {
            dialogSettings = false;
            if (user.Email == null && !confirmCodeEmailVisability)
                email = null;
            newPassword = null;
            repeatNewPassword = null;
        }

        public ICommand ConfirmEmailCommand { get; }
        private bool CanConfirmEmailCommandExecute(object p) => email?.Length > 0 && new Regex(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$").IsMatch(email);
        private void OnConfirmEmailCommandExecuted(object p)
        {
            try
            {
                if (context.Users.FirstOrDefault(u => u.Email == email) == null)
                {
                    code = new Random().Next(100000, 999999);
                    SendEmail(email, code);
                    confirmEmailButton = false;
                    confirmCodeEmailVisability = true;
                }
                else
                {
                    dialogText = "Адрес электронной почты занят.";
                    dialog = true;
                }
            }
            catch
            {
                dialogText = "Отсутствует подключение к интернету.";
                dialog = true;
            }
        }

        public ICommand ApplyDialogSettingsCommand { get; }
        private bool CanApplyDialogSettingsCommandExecute(object p) => !passErrorVisibility && (confirmCodeEmail?.Length > 0 || !confirmCodeEmailVisability);
        private void OnApplyDialogSettingsCommandExecuted(object p)
        {
            try
            {
                if (confirmCodeEmail?.Length > 0 && confirmCodeEmailVisability)
                {
                    if(code.ToString() == confirmCodeEmail)
                    {
                        context.Users.Find(user.Username).Email = email;
                        context.SaveChanges();
                        user = context.Users.Find(user.Username);
                        confirmCodeEmailVisability = false;
                        isUserEmailNull = true;
                    }
                    else
                    {
                        dialogText = "Неверный код.";
                        dialog = true;
                    }
                }
                if (newPassword?.Length > 0)
                {
                    if (newPassword == repeatNewPassword)
                    {
                        context.Users.Find(user.Username).Password = User.getHash(newPassword);
                        context.SaveChanges();
                        newPassword = null;
                        repeatNewPassword = null;
                    }
                    else
                    {
                        passError = "Пароли не совпадают.";
                        passErrorVisibility = true;
                    }
                }
            }
            catch
            {
                dialogText = "Отсутствует подключение к интернету.";
                dialog = true;
            }
        }

        public ICommand SwitchViewCommand { get; }
        private void OnSwitchViewCommandExecuted(object p)
        {
            try
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
            catch
            {
                dialogText = "Отсутствует подключение к интернету.";
                dialog = true;
            }
        }

        #endregion

        public MainWindowViewModel(User user, string view)
        {
            this.user = user;

            email = user.Email;
            _uploadBookMenuItemVisability = user.Role != UserRole.Moderator && user.Blocked == false;
            _contrloPaneMenuItemVisability = user.Role != UserRole.User && user.Role != UserRole.Author;
            _confirmEmailButton = user.Email == null;
            _isUserEmailNull = user.Email != null;

            TimeSpan durability = new TimeSpan(0, 0, 5);

            if (context.Books.FirstOrDefault(b => b.AuthorName == user.Username && b.Applied == BookStatus.Canceled) != null)
                Queue.Enqueue("Одна или несколько ваших книг не прошли модерацию.", null, null, null, false, false, durability);

            if (user.Email == null)
                Queue.Enqueue("У вас не указана электронная почта. Установить её можно в настройках аккаунта.", null, null, null, false, false, durability);

            WindowMinimizedCommand = new LambdaCommand(OnWindowMinimizedCommandExecuted);
            WindowMaximizeCommand = new LambdaCommand(OnWindowMaximizeCommandExecuted);
            SwitchUserCommand = new LambdaCommand(OnSwitchUserCommandExecuted);
            SwitchViewCommand = new LambdaCommand(OnSwitchViewCommandExecuted);
            CloseDialogCommand = new LambdaCommand(OnCloseDialogCommandExecuted);
            ShowDialogSettingsCommand = new LambdaCommand(OnShowDialogSettingsCommandExecuted);
            CloseDialogSettingsCommand = new LambdaCommand(OnCloseDialogSettingsCommandExecuted);
            ApplyDialogSettingsCommand = new LambdaCommand(OnApplyDialogSettingsCommandExecuted, CanApplyDialogSettingsCommandExecute);
            ConfirmEmailCommand = new LambdaCommand(OnConfirmEmailCommandExecuted, CanConfirmEmailCommandExecute);

            if (view == "MyBooks")
                selectedVM = new MyBooksViewModel(user, this);
            else
                selectedVM = new AllBooksViewModel(user, this);
        }
    }
}
