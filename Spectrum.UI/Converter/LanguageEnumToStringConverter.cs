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
            List<string> supportedLanguages = new List<string>();
            ObservableCollection<Language> languages = (ObservableCollection<Language>)value;

            foreach (Language language in languages)
            {
                string languageName = Enum.GetName(typeof(Language), language);

                supportedLanguages.Add(languageName);
            }

            return supportedLanguages;           
        }
        
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
