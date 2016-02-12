namespace TesseractUI.BusinessLogic.HOCR
{
    public interface ITesseractProgram
    {
        bool TesseractInstalled { get; }

        string GetTesseractProgramPath(string programDirectoryName, string exeName);
    }
}
