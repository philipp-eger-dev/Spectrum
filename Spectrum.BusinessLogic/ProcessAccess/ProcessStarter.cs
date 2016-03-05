using System;
using System.Diagnostics;

namespace TesseractUI.BusinessLogic.ProcessAccess
{
    public class ProcessStarter
    {
        #region Methods
        public void StartProcess(string FileToExecute, string Arguments)
        {
            Process p = new Process();
            ProcessStartInfo info = new ProcessStartInfo()
            {
                FileName = FileToExecute,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
                Arguments = Arguments
            };

            p.StartInfo = info;

            try
            {
                p.Start();
                p.WaitForExit();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
