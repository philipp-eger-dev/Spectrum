using TesseractUI.BusinessLogic.ProcessAccess;

namespace TesseractUI.BusinessLogic.HOCR
{
    public interface ITesseractProgram
    {
        bool TesseractInstalled { get; }

        string GetTesseractProgramPath(string programDirectoryName, string exeName);
        string GenerateHOCROfImage(ProcessStarter starter, string pdfImagePath, string tesseractLanguage);
    }
}
