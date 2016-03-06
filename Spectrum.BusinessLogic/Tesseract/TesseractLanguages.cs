using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TesseractUI.BusinessLogic.HOCR;

namespace TesseractUI.BusinessLogic.Tesseract
{
    public class TesseractLanguages
    {
        public List<Language> GetSupportedLanguagesFromTesseract(ITesseractProgram tesseractProgram)
        {
            List<Language> supportedTesseractLanguages = new List<Language>();

            string tesseractProgramPath = tesseractProgram.GetTesseractProgramPath(
                Properties.Settings.Default.ProgramDirectoryName, Properties.Settings.Default.ExeName);

            string tessDataDirectory = Path.GetDirectoryName(tesseractProgramPath) + "\\tessdata";

            List<string> tessDataFiles = Directory.GetFiles(tessDataDirectory).ToList();

            List<string> tessDataFileNames = new List<string>();

            foreach (var tessDataFile in tessDataFiles)
            {
                tessDataFileNames.Add(Path.GetFileName(tessDataFile));
            }

            var values = Enum.GetValues(typeof(Language)).Cast<Language>();

            List<string> languageStrings = new List<string>();
            LanguageMapping mapper = new LanguageMapping();

            foreach (Language item in values)
            {
                string languageString = mapper.GetTesseractLanguageStringFromEnumeration(item);

                var supportedLanguage = tessDataFileNames.Any(_ => _.StartsWith(languageString));

                if (supportedLanguage == true)
                {
                    supportedTesseractLanguages.Add(item);
                }
            }            

            return supportedTesseractLanguages;
        }
    }
}
