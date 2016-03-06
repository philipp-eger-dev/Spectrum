using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Threading;
using TesseractUI.BusinessLogic.HOCR;
using TesseractUI.BusinessLogic.Tesseract;

namespace TesseractUI
{
    public partial class MainWindow : Window
    {
        #region Fields
        private string _SelectedInputDirectory;
        private string _SelectedOutputDirectory;
        private int _FilesToProcess;

        public readonly FileGridViewModel FileGridVM;
        #endregion

        #region Constructor
        public MainWindow()
        {
            InitializeComponent();

            TesseractLanguages languages = new TesseractLanguages();
            languages.GetSupportedLanguagesFromTesseract(new TesseractProgram());

            this.FileGridVM = new FileGridViewModel();
            this.DataContext = this.FileGridVM;
        }
        #endregion

        #region UI Event Handler
        private void Button_Close_Application_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void Button_Select_InputFolder_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.ShowDialog();

            this._SelectedInputDirectory = dialog.SelectedPath;
            this._SelectedOutputDirectory = dialog.SelectedPath;

            TextBox_InputFolder.Text = _SelectedInputDirectory;
            TextBox_OutputFolder.Text = _SelectedOutputDirectory;

            if (!string.IsNullOrEmpty(TextBox_InputFolder.Text))
            {
                this.FillDataGridWithFileInfo(_SelectedInputDirectory);
                this.ChangeStatusMessage(_SelectedInputDirectory);   
            }
        }

        private void Button_Select_OutputFolder_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.ShowDialog();

            this._SelectedOutputDirectory = dialog.SelectedPath;
            TextBox_OutputFolder.Text = _SelectedInputDirectory;

            if (!string.IsNullOrEmpty(TextBox_OutputFolder.Text))
            {
                this.ChangeStatusMessage(_SelectedInputDirectory);
            }
        }

        private void Button_Start_Process_Click(object sender, RoutedEventArgs e)
        {
            ObservableCollection<FileToProcess> files = FileGridVM.Files;

            if (!String.IsNullOrEmpty(this._SelectedInputDirectory) && files.Any(_ => _.Process))
            {
                _FilesToProcess = files.Count;

                ProgressBar_Main.IsIndeterminate = true;
                RecognitionFactory recognition = new RecognitionFactory();

                foreach (var currentFile in files)
                {
                    if (currentFile.Process)
                    {
                        recognition.RecognitionFinished += recognition_RecognitionFinished;
                        recognition.ExecuteRecognitionAsync(currentFile, _SelectedOutputDirectory, FileGridVM.ReplaceSourceFile);   
                    }
                }   
            }
        }

        private void Button_Open_OutputFolder_OnClick(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(TextBox_OutputFolder.Text))
            {
                Process.Start(TextBox_OutputFolder.Text);
            }
        }

        private void Icon_About_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            AboutWindow aboutWindow = new AboutWindow
            {
                Owner = this
            };

            aboutWindow.ShowDialog();
        }
        #endregion

        #region Methods
        private void FillDataGridWithFileInfo(string directory)
        {
            this.FileGridVM.Files.Clear();

            foreach (string filePath in Directory.GetFiles(directory, "*.pdf"))
            {
                this.FileGridVM.Files.Add(new FileToProcess()
                {
                    FilePath = filePath,
                    ProcessingLanguage = TesseractUI.BusinessLogic.Tesseract.Language.German,
                    Status = ProcessingState.Pending,
                    Process = true
                });
            }
        }

        private void ChangeStatusMessage(string directory)
        {
            TextBlock_StatusInformation.Text = string.Format("{0} files to process in folder.",
                GetNumberOfFilesInDirectory(directory));
        }

        private int GetNumberOfFilesInDirectory(string directory)
        {
            return Directory.GetFiles(directory, "*.pdf").Count();
        }
        #endregion

        #region Event Handler
        private void recognition_RecognitionFinished(object sender, RecognitionEventArgs e)
        {
            System.Windows.Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                new Action(() =>
                {
                    if (e.File != null)
                    {
                        e.File.Status = ProcessingState.Complete;
                    }

                    _FilesToProcess -= 1;

                    TextBlock_StatusInformation.Text = _FilesToProcess == 0 ? 
                        "All files processed" : 
                        string.Format("{0} files to process", _FilesToProcess.ToString());

                    this.ProgressBar_Main.IsIndeterminate = false;
                }));
        }
        #endregion
    }
}
