using System.Collections.ObjectModel;

namespace TesseractUI
{
    public class FileGridViewModel
    {
        #region Properties
        public ObservableCollection<FileToProcess> Files { get; set; }
        public bool ReplaceSourceFile { get; set; }
        #endregion

        #region Constructor
        public FileGridViewModel()
        {
            this.Files = new ObservableCollection<FileToProcess>();
            this.ReplaceSourceFile = false;
        }
        #endregion
    }
}
