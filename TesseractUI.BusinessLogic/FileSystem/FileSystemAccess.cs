using System;
using System.IO;
using System.Linq;

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
        public string GetTesseractProgramPath(string ProgramDirectoryName, string ExeName)
        {
            var enviromentPath = Environment.GetEnvironmentVariable("PATH");

            var paths = enviromentPath.Split(';');
            var exePath = paths.Select(x => Path.Combine(x, ExeName))
                               .Where(x => File.Exists(x))
                               .FirstOrDefault();

            if (string.IsNullOrWhiteSpace(exePath) == false)
            {
                return exePath;
            }

            //Next check program files (64bit first)
            string ProgramW6432 = Environment.GetEnvironmentVariable("ProgramW6432");
            string pgFolder = Path.Combine(ProgramW6432 == null ? string.Empty : ProgramW6432, ProgramDirectoryName);
            string pg86Folder = Path.Combine(Environment.GetEnvironmentVariable("ProgramFiles"), ProgramDirectoryName);

            if (ProgramDirectoryName != null && ProgramDirectoryName != "")
            {
                if (Directory.Exists(pgFolder))
                {
                    string[] gsfiles = Directory.GetFiles(pgFolder, ExeName, SearchOption.AllDirectories);

                    foreach (string gs in gsfiles)
                        return gs;
                }

                if (Directory.Exists(pg86Folder))
                {
                    string[] gsfiles = Directory.GetFiles(pg86Folder, ExeName, SearchOption.AllDirectories);

                    foreach (string gs in gsfiles)
                        return gs;
                }
            }
            //Finally check directory of executing assembly
            string[] files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, ExeName, SearchOption.AllDirectories);

            foreach (string gs in files)
                return gs;

            return null;
        }
        #endregion
    }
}
