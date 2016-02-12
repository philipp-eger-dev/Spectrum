using System;
using System.Configuration;
using System.IO;
using System.Linq;
using TesseractUI.BusinessLogic.ProcessAccess;

namespace TesseractUI.BusinessLogic.HOCR
{
    public class TesseractProgram : ITesseractProgram
    {
        public bool TesseractInstalled
        {
            get
            {
                string programDirectoryName = ConfigurationManager.AppSettings["ProgramDirectoryName"];
                string exeName = ConfigurationManager.AppSettings["ExeName"];

                string result = new TesseractProgram().GetTesseractProgramPath(programDirectoryName, exeName);

                if (result != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public string GenerateHOCROfImage(ProcessStarter starter, string pdfImagePath, string tesseractLanguage)
        {
            string outputFile = pdfImagePath.Replace(Path.GetExtension(pdfImagePath), "");

            string oArg = '"' + outputFile + '"';
            string commandArgs =
                string.Concat(pdfImagePath, " ", oArg, " -l " + tesseractLanguage + " -psm 1 hocr ");

            starter.StartProcess(this.GetTesseractProgramPath(
                    Properties.Settings.Default.ProgramDirectoryName,
                    Properties.Settings.Default.ExeName), commandArgs);

            return outputFile;
        }

        public string GetTesseractProgramPath(string programDirectoryName, string exeName)
        {
            string enviromentPath = Environment.GetEnvironmentVariable("PATH");

            string[] paths = enviromentPath.Split(';');
            string exePath = paths.Select(x => Path.Combine(x, exeName))
                .Where(x => File.Exists(x))
                .FirstOrDefault();

            if (string.IsNullOrWhiteSpace(exePath) == false)
            {
                return exePath;
            }

            string ProgramW6432 = Environment.GetEnvironmentVariable("ProgramW6432");
            string pgFolder = Path.Combine(ProgramW6432 == null ? string.Empty : ProgramW6432, programDirectoryName);
            string pg86Folder = Path.Combine(Environment.GetEnvironmentVariable("ProgramFiles"), programDirectoryName);

            if (programDirectoryName != null && programDirectoryName != "")
            {
                if (Directory.Exists(pgFolder))
                {
                    string[] gsfiles = Directory.GetFiles(pgFolder, exeName, SearchOption.AllDirectories);

                    foreach (string gs in gsfiles)
                        return gs;
                }

                if (Directory.Exists(pg86Folder))
                {
                    string[] gsfiles = Directory.GetFiles(pg86Folder, exeName, SearchOption.AllDirectories);

                    foreach (string gs in gsfiles)
                        return gs;
                }
            }
            //Finally check directory of executing assembly
            string[] files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, exeName, SearchOption.AllDirectories);

            foreach (string gs in files)
                return gs;

            return null;
        }
    }
}
