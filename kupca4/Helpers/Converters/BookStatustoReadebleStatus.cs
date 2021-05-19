using kupca4.DB;
using System;
using System.Windows.Data;
using System.Drawing;

namespace kupca4.Helpers.Converters
{
    class BookStatustoReadebleStatus : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            switch ((BookStatus)value)
            {
                case BookStatus.NeedModer:
                    return "На модерации";
                case BookStatus.Canceled:
                    return "Требует изменений";
                case BookStatus.Banned:
                    return "Заблокирована";
                default:
                    return "Опубликовано";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter,
           System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}

