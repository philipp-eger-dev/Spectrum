using Clock.Hocr;
using System;
using System.Collections.Generic;
using System.IO;
using TesseractUI.BusinessLogic.Exceptions;
using TesseractUI.BusinessLogic.ProcessAccess;

namespace TesseractUI.BusinessLogic.HOCR
{
    public class HOCRFileCreator
    {
        public hDocument CreateHOCROfImages(ITesseractProgram tesseract, ProcessStarter starter, 
            List<string> pdfImagePaths, string tesseractLanguage)
        {
            if (tesseract == null || starter == null || pdfImagePaths == null || string.IsNullOrEmpty(tesseractLanguage))
            {
                throw new ArgumentNullException();
            }
            if (!tesseract.TesseractInstalled)
            {
                throw new TesseractNotInstalledException("Tesseract not installed");
            }

            hDocument documentWithHocr = new hDocument();

            foreach (string pdfImagePath in pdfImagePaths)
            {
                string outputFile = tesseract.GenerateHOCROfImage(starter, pdfImagePath, tesseractLanguage);

                documentWithHocr.AddFile(outputFile + Properties.Settings.Default.HOCRFileExtension);
            }

            return documentWithHocr;
        }
    }
}
