using kupca4.DB;
using System;
using System.Windows.Data;

namespace kupca4.Helpers.Converters
{
    class GenreIDtoGenreNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            using (KP_LibraryContext context = new KP_LibraryContext())
            {
                return context.Genres.Find((int)value).Genrename;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter,
           System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
