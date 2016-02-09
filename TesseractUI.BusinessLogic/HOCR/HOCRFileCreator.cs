using Clock.Hocr;
using System.Collections.Generic;
using System.IO;
using TesseractUI.BusinessLogic.FileSystem;
using TesseractUI.BusinessLogic.ProcessAccess;

namespace TesseractUI.BusinessLogic.HOCR
{
    public class HOCRFileCreator
    {
        public hDocument CreateHOCROfImage(IFileSystem fileSystem, List<string> pdfImagePaths, string tesseractLanguage)
        {
            hDocument documentWithHocr = new hDocument();

            foreach (string pdfImagePath in pdfImagePaths)
            {
                string outputFile = pdfImagePath.Replace(Path.GetExtension(pdfImagePath), "");

                string oArg = '"' + outputFile + '"';
                string commandArgs =
                    string.Concat(pdfImagePath, " ", oArg, " -l " + tesseractLanguage + " -psm 1 hocr ");
                new ProcessStarter().StartProcess(
                    fileSystem.GetTesseractProgramPath("Tesseract-OCR", "tesseract.exe"), commandArgs);

                documentWithHocr.AddFile(outputFile + ".hocr");
            }

            return documentWithHocr;
        }
    }
}
