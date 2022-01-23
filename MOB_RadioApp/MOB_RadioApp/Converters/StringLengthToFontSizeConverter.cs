using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace MOB_RadioApp.Converters
{
    public class StringLengthToFontSizeConverter : IValueConverter
    {
        /// <summary>
        /// a small stringlength returns a large fontsize
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // 1 means we're in stationscontrol
            if ((string)parameter == "1")
            {
                string p = (string)value;
                int stringLength = p.Length;
                if (stringLength > 30)
                    return 9;
                else if (stringLength > 25)
                    return 10;
                else if (stringLength > 20)
                    return 11;
                else if (stringLength > 15)
                    return 12;
                else if (stringLength > 10)
                    return 13;
                else if (stringLength > 5)
                    return 15;
                else return 15;
            }

            // 2 means favouritescontrol, fonts are larger
            if ((string)parameter == "2")
            {
                string p = (string)value;
                int stringLength = p.Length;
                if (stringLength > 30)
                    return 12;
                else if (stringLength > 25)
                    return 13;
                else if (stringLength > 20)
                    return 14;
                else if (stringLength > 15)
                    return 15;
                else if (stringLength > 10)
                    return 16;
                else if (stringLength > 5)
                    return 17;
                else return 17;
            }
             // 2 means favouritescontrol, fonts are larger
            if ((string)parameter == "3")
            {
                string p = (string)value;
                int stringLength = p.Length;
                if (stringLength > 30)
                    return 32;
                else if (stringLength > 25)
                    return 33;
                else if (stringLength > 20)
                    return 34;
                else if (stringLength > 15)
                    return 35;
                else if (stringLength > 10)
                    return 36;
                else if (stringLength > 5)
                    return 37;
                else return 37;
            }
            else
            {
                if (value != null)
                {
                    // else we're in the player, font is much larger
                    string p = (string)value;
                    int stringLength = p.Length;
                    if (stringLength > 30)
                        return 35;
                    else if (stringLength > 25)
                        return 35;
                    else if (stringLength > 20)
                        return 34;
                    else if (stringLength > 15)
                        return 35;
                    else if (stringLength > 10)
                        return 35;
                    else if (stringLength > 5)
                        return 35;
                    else return 35;
                }
                return 20;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
