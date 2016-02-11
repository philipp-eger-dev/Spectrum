using Clock.Hocr;
using System.Collections.Generic;
using System.IO;
using TesseractUI.BusinessLogic.ProcessAccess;

namespace TesseractUI.BusinessLogic.HOCR
{
    public class HOCRFileCreator
    {
        public hDocument CreateHOCROfImage(TesseractProgram tesseract, ProcessStarter starter, 
            List<string> pdfImagePaths, string tesseractLanguage)
        {
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
