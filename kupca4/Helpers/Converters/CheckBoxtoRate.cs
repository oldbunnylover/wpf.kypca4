using kupca4.DB;
using System;
using System.Linq;
using System.Windows.Data;

namespace kupca4.Helpers.Converters
{
    class CheckBoxtoRate : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (Int32.Parse((string)parameter) <= (int)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter,
           System.Globalization.CultureInfo culture)
        {
            return Int32.Parse((string)parameter);
        }
    }
}
