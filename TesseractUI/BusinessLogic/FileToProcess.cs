namespace TesseractUI
{
    public class FileToProcess
    {
        #region Properties
        public string FilePath { get; set; }
        public ProcessingState Status { get; set; }
        public Language ProcessingLanguage { get; set; }
        public bool Process { get; set; }
        #endregion
    }
}
