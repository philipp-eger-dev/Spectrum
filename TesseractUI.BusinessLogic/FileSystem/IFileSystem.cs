namespace TesseractUI.BusinessLogic.FileSystem
{
    public interface IFileSystem
    {
        string GetTesseractProgramPath(string ProgramDirectoryName, string ExeName);
    }
}
