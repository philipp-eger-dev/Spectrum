using System;
using System.IO;

namespace TesseractUI.BusinessLogic.FileSystem
{
    public class FileSystemAccess : IFileSystem
    {
        #region Fields
        private string _TemporaryWorkingDirectory;
        #endregion

        #region Properties
        public string DestinationPDFPath
        {
            get
            {
                return this.TemporaryWorkingDirectory + "\\" + Path.GetFileName(this.SourcePDFPath);
            }
        }

        public string TemporaryWorkingDirectory
        {
            get
            {
                if (string.IsNullOrEmpty(this._TemporaryWorkingDirectory))
                {
                    this._TemporaryWorkingDirectory = GetTemporaryDirectory();
                }

                return this._TemporaryWorkingDirectory;
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

        private string GetTemporaryDirectory()
        {
            string tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDirectory);

            return tempDirectory;
        }

        public void DeleteTemporaryWorkingDirectory()
        {
            try
            {
                Directory.Delete(this.TemporaryWorkingDirectory, true);
            }
            catch (Exception)
            {

            }
        }
        #endregion
    }
}
