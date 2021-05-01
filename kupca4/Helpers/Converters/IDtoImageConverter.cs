using System;
using System.IO;
using System.Windows.Data;

namespace kupca4.Helpers.Converters
{
    class IDtoImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Personal) + $@"\DuckLibrary\books\{(int)value}\cover.jpg"))
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.Personal) + $@"\DuckLibrary\books\{(int)value}\cover.jpg";
            } 
            else
            {
                return "/Styles/img/noPhoto.png";
            }
            
        }

        public object ConvertBack(object value, Type targetType, object parameter,
           System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
