using System;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace kupca4.Helpers.Converters
{
    class IDtoImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                BitmapImage imgTemp = new BitmapImage();
                imgTemp.BeginInit();
                imgTemp.CacheOption = BitmapCacheOption.OnLoad;
                //imgTemp.UriSource = new Uri($"http://localhost:3000/books/{(int)value}/cover.png");
                imgTemp.UriSource = new Uri($"http://192.168.0.100:3000/books/{(int)value}/cover.png");
                imgTemp.EndInit();
                return imgTemp;
            }
            catch
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
