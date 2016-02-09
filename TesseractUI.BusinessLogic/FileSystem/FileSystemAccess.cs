using System;
using System.IO;
using System.Linq;

namespace TesseractUI.BusinessLogic.FileSystem
{
    public class FileSystemAccess : IFileSystem
    {
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
    }
}
