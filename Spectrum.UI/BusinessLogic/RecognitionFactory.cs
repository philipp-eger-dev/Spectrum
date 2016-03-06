using System;
using System.IO;
using System.Threading;
using TesseractUI.BusinessLogic;
using TesseractUI.BusinessLogic.Tesseract;

namespace TesseractUI
{
    public class RecognitionFactory
    {
        #region Consts
        private const string FILENAMEEXTENSION_OCR = "_OCR";
        #endregion

        #region Events
        public event EventHandler<RecognitionEventArgs> RecognitionFinished; 
        #endregion

        #region Methods
        public void ExecuteRecognitionAsync(FileToProcess file, string outputDirectory, bool replaceSourceFile)
        {
            Thread recognitionThread = new Thread(() =>
            {
                file.Status = ProcessingState.Processing;
                 
                PDFDocument document = new PDFDocument(file.FilePath);
                
                //TODO Support multiple languages
                document.Ocr(Language.German);
                document.SaveToPath(this.GetTargetPath(file.FilePath, outputDirectory, replaceSourceFile));
                document.DeleteTemporaryFiles();

                if (this.RecognitionFinished != null)
                {
                    this.RecognitionFinished(this, new RecognitionEventArgs(file));                    
                }
            });

            recognitionThread.IsBackground = true;
            recognitionThread.Start();
        }

        private string GetTargetPath(string filePath, string outputDirectory, bool replaceSourceFiles)
        {
            string targetFileName = Path.GetFileName(filePath);

            if (Path.GetDirectoryName(filePath) == outputDirectory && !replaceSourceFiles)
            {
                 targetFileName = 
                    Path.GetFileNameWithoutExtension(filePath) + "_OCR" + Path.GetExtension(filePath);
            }
            else if (replaceSourceFiles)
            {
                targetFileName = Path.GetFileName(filePath);
            }

            return outputDirectory + "\\" + targetFileName;
        }
        #endregion
    }
}
