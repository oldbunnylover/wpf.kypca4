using System;
using System.Net;
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
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"http://localhost:3000/books/{(int)value}/cover.png");
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    BitmapImage imgTemp = new BitmapImage();
                    imgTemp.BeginInit();
                    imgTemp.CacheOption = BitmapCacheOption.OnLoad;
                    imgTemp.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                    imgTemp.UriSource = new Uri($"http://localhost:3000/books/{(int)value}/cover.png");
                    imgTemp.EndInit();
                    response.Close();
                    return imgTemp;
                }
                else
                {
                    response.Close();
                    return "/Styles/img/noPhoto.png";
                }
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
