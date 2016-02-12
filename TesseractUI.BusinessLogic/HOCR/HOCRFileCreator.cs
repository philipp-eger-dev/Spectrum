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
        public hDocument CreateHOCROfImage(ITesseractProgram tesseract, ProcessStarter starter, 
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
                string outputFile = pdfImagePath.Replace(Path.GetExtension(pdfImagePath), "");

                string oArg = '"' + outputFile + '"';
                string commandArgs =
                    string.Concat(pdfImagePath, " ", oArg, " -l " + tesseractLanguage + " -psm 1 hocr ");

                starter.StartProcess(tesseract.GetTesseractProgramPath(
                        Properties.Settings.Default.ProgramDirectoryName, 
                        Properties.Settings.Default.ExeName), commandArgs);

                documentWithHocr.AddFile(outputFile + Properties.Settings.Default.HOCRFileExtension);
            }

            return documentWithHocr;
        }
    }
}
