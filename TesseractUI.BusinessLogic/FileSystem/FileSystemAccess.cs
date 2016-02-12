using System;
using System.IO;

namespace TesseractUI.BusinessLogic.FileSystem
{
    public class FileSystemAccess : IFileSystem
    {
        #region Properties
        public string DestinationPDFPath
        {
            get
            {
                return this.SourcePDFPath.Replace(
                    Path.GetExtension(this.SourcePDFPath), "") + "_OCR.pdf";
            }
        }

        public string OutputDirectory
        {
            get
            {
                return Path.GetDirectoryName(this.SourcePDFPath);
            }
        }

        public string SourcePDFPath { get; private set; }
        #endregion

        #region Constructor
        public FileSystemAccess(string sourcePath)
        {
            this.SourcePDFPath = sourcePath;
        }
        #endregion

        #region Methods
        public bool Exists(string filePath)
        {
            return File.Exists(filePath);
        }
        #endregion
    }
}
