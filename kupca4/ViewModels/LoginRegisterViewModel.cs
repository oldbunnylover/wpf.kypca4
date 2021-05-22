using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using kupca4.DB;
using kupca4.Helpers.Commands;
using kupca4.ViewModels.Base;

namespace kupca4.ViewModels
{
    class LoginRegisterViewModel : ViewModel
    {
        #region private fields
        private readonly KP_LibraryContext context = new KP_LibraryContext();
        private string _name;
        private string _surname;
        private string _registerUsername;
        private string _registerPassword;
        private string _loginUsername;
        private string _loginPassword;
        private bool _loading = false;
        private bool _dialog = false;
        private bool _errorMsg = false;
        private bool _closeDialogButtonVisibility = false;
        private string _dialogText;
        private string _passError;
        private string _reserUsername;
        private string _resetEmail;
        private bool _passErrorVisibility = false;
        private bool _resetPasswordVisiblity = false;

        #endregion

        #region public fields

        public string passError
        {
            get => _passError;
            set => Set(ref _passError, value);
        }

        public string resetUsername
        {
            get => _reserUsername;
            set => Set(ref _reserUsername, value);
        }

        public string resetEmail
        {
            get => _resetEmail;
            set => Set(ref _resetEmail, value);
        }

        public bool errorMsg
        {
            get => _errorMsg;
            set => Set(ref _errorMsg, value);
        }

        public bool closeDialogButtonVisibility
        {
            get => _closeDialogButtonVisibility;
            set => Set(ref _closeDialogButtonVisibility, value);
        }

        public bool passErrorVisibility
        {
            get => _passErrorVisibility;
            set => Set(ref _passErrorVisibility, value);
        }
        
        public bool resetPasswordVisiblity
        {
            get => _resetPasswordVisiblity;
            set => Set(ref _resetPasswordVisiblity, value);
        }

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

        public string registerPassword
        {
            get => _registerPassword;
            set
            {
                Set(ref _registerPassword, value);
                passErrorVisibility = false;
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
                    passError = "Пароль может содержать только латиницу, цифры и специальные символы('!', '?', '.', '@')";
                    passErrorVisibility = true;
                }
            }
        }

        public string loginUsername
        {
            get => _loginUsername;
            set => Set(ref _loginUsername, value);
        }

        public string loginPassword
        {
            get => _loginPassword;
            set => Set(ref _loginPassword, value);
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

        #region Checks

        private bool ServerCheck()
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://localhost:3000/");
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    response.Close();
                    return true;
                }
                else
                {
                    dialogText = "Отсутствует подключение к серверу.";
                    dialog = true;
                    response.Close();
                    return false;
                }
            }
            catch
            {
                dialogText = "Отсутствует подключение к серверу.";
                dialog = true;
            }
            return false;
        }

        async void DataBaseAsync()
        {
            await Task.Run(() => {
                errorMsg = false;
                closeDialogButtonVisibility = true;
                if (!new KP_LibraryContext(true).connection)
                {
                    dialogText = "Отсутствует подключение к интернету.";
                    errorMsg = true;
                    closeDialogButtonVisibility = false;
                    dialog = true;
                }
            });
        }

        #endregion

        #region mailSend

        private void SendEmail(string mail, string username, string password)
        {
            try
            {
                MailAddress from = new MailAddress("ducklibrarynoreply@gmail.com", "DuckLibrary");
                MailAddress to = new MailAddress(mail);
                MailMessage m = new MailMessage(from, to);
                m.Subject = "Восстановление пароля DuckLibrary";
                m.Body = $"Здравствуйте! Вот ваш новый пароль для аккаунта {username}:\n{password}";
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

        public ICommand RegisterCommand { get; }
        private bool CanRegisterCommandExecute(object p) => name?.Length > 1 && name?.Length < 16 && surname?.Length > 1 && surname?.Length < 21
            && registerUsername?.Length > 3 && registerUsername?.Length < 16 && registerPassword?.Length > 0 && !passErrorVisibility 
            && new Regex("[a-zA-Zа-яА-я]$").IsMatch(name) && new Regex("[a-zA-Zа-яА-я]$").IsMatch(surname) && new Regex("[a-zA-Z0-9]$").IsMatch(registerUsername);

        private void OnRegisterCommandExecuted(object p)
        {
            if (ServerCheck())
            {
                try
                {
                    if (context.Users.FirstOrDefault(u => u.Username == registerUsername) == null)
                    {
                        User user = new User(name, surname, registerUsername, registerPassword);
                        context.Users.Add(user);
                        context.SaveChanges();
                        var MainWindowViewModel = new MainWindowViewModel(user, "AllBooks");
                        var MainWindow = new MainWindow
                        {
                            DataContext = MainWindowViewModel
                        };
                        MainWindow.Show();
                        (p as Window).Close();
                    }
                    else
                    {
                        dialogText = "Пользователь с данным псевдонимом уже зарегистрирован.";
                        dialog = true;
                    }
                }
                catch
                {
                    dialogText = "Отсутствует подключение к интернету.";
                    dialog = true;
                }
               
            }
        }

        public ICommand LoginCommand { get; }
        private bool CanLoginCommandExecute(object p) => loginUsername?.Length > 0 && loginPassword?.Length > 0;
        private void OnLoginCommandExecuted(object p)
        {
            if (ServerCheck())
            {
                try
                {
                    if (context.Users.FirstOrDefault(u => u.Username == loginUsername && u.Password == User.getHash(loginPassword)) != null)
                    {
                        User user = context.Users.FirstOrDefault(u => u.Username == loginUsername);
                        var MainWindowViewModel = new MainWindowViewModel(user, "MyBooks");
                        var MainWindow = new MainWindow
                        {
                            DataContext = MainWindowViewModel
                        };
                        MainWindow.Show();
                        (p as Window).Close();
                    }
                    else
                    {
                        loading = false;
                        dialogText = "Неверный логин или пароль.";
                        dialog = true;
                    }
                }
                catch
                {
                    dialogText = "Отсутствует подключение к интернету.";
                    dialog = true;
                }
            }
        }

        public ICommand CloseDialogCommand { get; }
        private void OnCloseDialogCommandExecuted(object p) => dialog = false;

        public ICommand OpenResetPasswordCommand { get; }
        private void OnOpenResetPasswordCommandExecuted(object p) => resetPasswordVisiblity = true;

        public ICommand CloseResetPasswordCommand { get; }
        private void OnCloseResetPasswordCommandExecuted(object p)
        {
            resetEmail = null;
            resetUsername = null;
            resetPasswordVisiblity = false;
        }

        public ICommand TryAgainCommand { get; }
        private void OnTryAgainCommandExecuted(object p)
        {
            dialog = false;
            DataBaseAsync();
        }

        public ICommand ResetPasswordCommand { get; }
        private bool CanResetPasswordCommandExecute(object p) => resetEmail?.Length > 0 && resetUsername?.Length > 0 && new Regex(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$").IsMatch(resetEmail);
        private void OnResetPasswordCommandExecuted(object p)
        {
            try
            {
                if (context.Users.FirstOrDefault(u => u.Username == resetUsername && u.Email == resetEmail) != null)
                {
                    string password = User.getHash(new Random().Next(1, 10000).ToString()).Substring(10);
                    SendEmail(resetEmail, resetUsername, password);
                    context.Users.Find(resetUsername).Password = User.getHash(password);
                    context.SaveChanges();

                    OnCloseResetPasswordCommandExecuted(true);
                    dialogText = "Пароль успешно сброшен. Проверьте свою почту.";
                    dialog = true;
                }
                else
                {
                    dialogText = "Пользователь с такими данными не зарегистрирован.";
                    dialog = true;
                }
            }
            catch
            {
                dialogText = "Отсутствует подключение к интернету.";
                dialog = true;
            }
        }

        #endregion

        public LoginRegisterViewModel()
        {
            #region Commands

            RegisterCommand = new LambdaCommand(OnRegisterCommandExecuted, CanRegisterCommandExecute);
            CloseDialogCommand = new LambdaCommand(OnCloseDialogCommandExecuted);
            OpenResetPasswordCommand = new LambdaCommand(OnOpenResetPasswordCommandExecuted);
            CloseResetPasswordCommand = new LambdaCommand(OnCloseResetPasswordCommandExecuted);
            TryAgainCommand = new LambdaCommand(OnTryAgainCommandExecuted);
            LoginCommand = new LambdaCommand(OnLoginCommandExecuted, CanLoginCommandExecute);
            ResetPasswordCommand = new LambdaCommand(OnResetPasswordCommandExecuted, CanResetPasswordCommandExecute);

            #endregion

            DataBaseAsync();
        }
    }
}
