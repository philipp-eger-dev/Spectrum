using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
