using System.Collections.ObjectModel;
using TesseractUI.BusinessLogic.Tesseract;

namespace TesseractUI
{
    public class FileToProcess
    {
        #region Properties
        public string FilePath { get; set; }
        public ProcessingState Status { get; set; }
        public Language ProcessingLanguage { get; set; }
        public bool Process { get; set; }
        public ObservableCollection<Language> SupportedLanguages { get; set; }
        #endregion

        #region Constructor
        public FileToProcess()
        {
            this.ProcessingLanguage = Language.English;
        }
        #endregion
    }
}
