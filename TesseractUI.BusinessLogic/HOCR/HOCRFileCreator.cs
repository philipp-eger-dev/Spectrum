using Clock.Hocr;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
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
            Contract.Requires<ArgumentNullException>(tesseract != null, "tesseract");
            Contract.Requires<ArgumentNullException>(starter != null, "starter");
            Contract.Requires<ArgumentNullException>(pdfImagePaths != null, "pdfImagePaths");
            Contract.Requires<ArgumentNullException>(string.IsNullOrEmpty(tesseractLanguage), "tesseractLanguage");

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
