namespace TesseractUI
{
    public class RecognitionEventArgs
    {
        #region Properties
        public FileToProcess File { get; private set; }
        #endregion

        #region Constructor
        public RecognitionEventArgs(FileToProcess file)
        {
            this.File = file;
        }
        #endregion
    }
}
