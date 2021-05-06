using System;
using System.Linq;
using System.Windows.Data;
using kupca4.DB;

namespace kupca4.Helpers.Converters
{
    class IDtoRateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            using (KP_LibraryContext context = new KP_LibraryContext())
            {
                if (context.BookRates.FirstOrDefault(b => b.BookId == (int)value) != null)
                {
                    return (double)context.BookRates.Where(b => b.BookId == (int)value).Sum(b => b.UserRate) / (double)context.BookRates.Where(b => b.BookId == (int)value).Count();
                } else
                {
                    return 0;
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter,
           System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
