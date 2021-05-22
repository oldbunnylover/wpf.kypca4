using System.Windows.Controls;
using System.Globalization;
using System.Text.RegularExpressions;

namespace kupca4.Helpers.Validation
{
    class RegexRule : ValidationRule
    {
        public string fieldName { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string charString = GetBound.GetBoundValue(value) as string;
            string msg;
            Regex regex = new Regex("^");

            switch (fieldName)
            {
                case "Login":
                    msg = "Поле может содержать только цифры и латиницу.";
                    regex = new Regex("[a-zA-Z0-9]$");
                    break;
                case "SurnameName":
                    msg = "Поле может содержать только кириллицу и латиницу.";
                    regex = new Regex("[a-zA-Zа-яА-я]$");
                    break;
                case "Email":
                    msg = "Неверный формат электронной почты.";
                    regex = new Regex(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");
                    break;
                default:
                    msg = "Некорректный ввод.";
                    break;
            }

            if (regex.IsMatch(charString))
            {
                return ValidationResult.ValidResult;
            }

            return new ValidationResult(false, msg);

        }
    }
}
