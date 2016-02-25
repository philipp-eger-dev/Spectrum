using System;
using System.Collections.Generic;
using System.IO;
using TesseractUI.BusinessLogic.Exceptions;
using TesseractUI.BusinessLogic.FileSystem;
using TesseractUI.BusinessLogic.ProcessAccess;

namespace TesseractUI.BusinessLogic.HOCR
{
    public class HOCRFileCreator
    {
        public IHOCRDocument CreateHOCROfImages(IHOCRDocument document, 
            IParser parser, IFileSystem fileSystem, ITesseractProgram tesseract, ProcessStarter starter, 
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

            foreach (string pdfImagePath in pdfImagePaths)
            {
                if (!fileSystem.Exists(pdfImagePath)){
                    throw new FileNotFoundException("PdfImagePath");
                }

                string outputFile = tesseract.GenerateHOCROfImage(starter, pdfImagePath, tesseractLanguage);

                document.AddFile(parser, outputFile + Properties.Settings.Default.HOCRFileExtension);
            }

            return document;
        }
    }
}
