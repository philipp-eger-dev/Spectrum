using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Data;
using TesseractUI.BusinessLogic.Tesseract;

namespace TesseractUI
{
    public class LanguageEnumToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {            
            return value;           
        }
        
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
