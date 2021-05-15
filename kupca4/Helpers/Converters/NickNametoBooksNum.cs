using kupca4.DB;
using System;
using System.Linq;
using System.Windows.Data;

namespace kupca4.Helpers.Converters
{
    class NickNametoBooksNum : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            using (KP_LibraryContext context = new KP_LibraryContext())
            {
                return context.Books.Where(b => b.AuthorName == (string)value && b.Applied == BookStatus.Applied).Count();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter,
           System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
