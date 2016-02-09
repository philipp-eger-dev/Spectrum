namespace TesseractUI.BusinessLogic.FileSystem
{
    public interface IFileSystem
    {
        string OutputDirectory { get; }
        string SourcePDFPath { get; }
        string DestinationPDFPath { get; }

        string GetTesseractProgramPath(string ProgramDirectoryName, string ExeName);
    }
}
