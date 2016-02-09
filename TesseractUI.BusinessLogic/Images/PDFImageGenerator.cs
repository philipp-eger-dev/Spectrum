using TesseractUI.BusinessLogic.FileSystem;

namespace TesseractUI.BusinessLogic.Images
{
    public class PDFImageGenerator
    {
        private IFileSystem _FileSystemExporter;

        public PDFImageGenerator(IFileSystem fileSystemExporter)
        {
            this._FileSystemExporter = fileSystemExporter;
        }
    }
}
