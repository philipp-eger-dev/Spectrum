using System.Collections.ObjectModel;
using TesseractUI.BusinessLogic.Tesseract;

namespace TesseractUI
{
    public class FileToProcess
    {
        private Language _ProcessingLanguage;

        #region Properties
        public string FilePath { get; set; }
        public ProcessingState Status { get; set; }
        public Language ProcessingLanguage { get
            {
                return this._ProcessingLanguage;
            }
            set
            {
                this._ProcessingLanguage = value;
            }
        }
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
