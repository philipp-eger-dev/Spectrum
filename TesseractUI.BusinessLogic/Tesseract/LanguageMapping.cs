using System;

namespace TesseractUI.BusinessLogic.Tesseract
{
    /// <summary>
    /// Maps language enum to tesseract language strings for simpler API usage
    /// </summary>
    public class LanguageMapping
    {
        /// <summary>
        /// Returns tesseract language string from language string enumeration
        /// </summary>
        /// <param name="lang">Language from Language enumeration</param>
        /// <returns>Tesseract language string</returns>
        public string GetTesseractLanguageStringFromEnumeration(Language lang)
        {
            switch (lang)
            {
                case Language.English:
                    return "eng";
                case Language.German:
                    return "deu";
                default:
                    throw new Exception("Language not found");
            }
        }
    }
}
