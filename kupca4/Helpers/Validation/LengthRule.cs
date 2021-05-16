using System.Windows.Controls;
using System.Globalization;

namespace kupca4.Helpers.Validation
{
    class LengthRule : ValidationRule
    {
        public int MinimumLength { get; set; }
        public int MaximumLength { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string charString = GetBound.GetBoundValue(value) as string;

            if (charString.Length < MinimumLength)
            {
                if (MinimumLength != 2 && MinimumLength != 3 && MinimumLength != 4)
                    return new ValidationResult(false, $"Поле должно содержать минимум {MinimumLength} символов.");
                else
                    return new ValidationResult(false, $"Поле должно содержать минимум {MinimumLength} символа.");
            }

            if (charString.Length > MaximumLength && MaximumLength != 2 && MaximumLength != 3 && MaximumLength != 4)
            {
                if (MaximumLength != 2 && MaximumLength != 3 && MaximumLength != 4)
                    return new ValidationResult(false, $"Поле должно содержать максимум {MaximumLength} символов.");
                else
                    return new ValidationResult(false, $"Поле должно содержать максимум {MaximumLength} символа.");

            }

            return ValidationResult.ValidResult;
        }
    }
}
