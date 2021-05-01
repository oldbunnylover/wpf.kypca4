using kupca4.DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Windows.Data;

namespace kupca4.Helpers.Converters
{
    class NicktoFullName : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            using (KP_LibraryContext context = new KP_LibraryContext())
            {
                return context.Users.FirstOrDefault(u => u.Username == value.ToString()).Fullname + " (" + value.ToString() + ")";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter,
           System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
