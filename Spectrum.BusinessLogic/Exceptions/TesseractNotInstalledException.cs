using System;

namespace TesseractUI.BusinessLogic.Exceptions
{
    public class TesseractNotInstalledException : Exception
    {
        public TesseractNotInstalledException(string message) : base(message)
        {
        }
    }
}
