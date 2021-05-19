using kupca4.DB;
using System;
using System.Windows.Data;
using System.Drawing;

namespace kupca4.Helpers.Converters
{
    class AppliedStatusToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            switch ((BookStatus)value)
            {
                case BookStatus.NeedModer:
                    return "#B2080808";
                case BookStatus.Canceled:
                    return "#99D15656";
                case BookStatus.Banned:
                    return "#CC590000";
                default:
                    return Color.Transparent;
            }
        }
        
        public object ConvertBack(object value, Type targetType, object parameter,
           System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
