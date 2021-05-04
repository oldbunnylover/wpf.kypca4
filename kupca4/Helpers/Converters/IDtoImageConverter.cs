using System;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace kupca4.Helpers.Converters
{
    class IDtoImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Personal) + $@"\DuckLibrary\books\{(int)value}\cover.jpg"))
            {
                BitmapImage imgTemp = new BitmapImage();
                imgTemp.BeginInit();
                imgTemp.CacheOption = BitmapCacheOption.OnLoad;
                imgTemp.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                imgTemp.UriSource = new Uri(Environment.GetFolderPath(Environment.SpecialFolder.Personal) + $@"\DuckLibrary\books\{(int)value}\cover.jpg");
                imgTemp.EndInit();
                return imgTemp;
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
