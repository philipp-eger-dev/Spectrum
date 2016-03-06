namespace TesseractUI.BusinessLogic.FileSystem
{
    public interface IFileSystem
    {
        string OutputDirectory { get; }
        string SourcePDFPath { get; }
        string DestinationPDFPath { get; }
        string TemporaryWorkingDirectory { get; }

        bool Exists(string filePath);
        void DeleteTemporaryWorkingDirectory();
    }
}
