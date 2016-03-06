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
                document.SaveToPath(this.GetTargetPath(file.FilePath, outputDirectory));
                document.DeleteTemporaryFiles();

                if (this.RecognitionFinished != null)
                {
                    this.RecognitionFinished(this, new RecognitionEventArgs(file));                    
                }
            });

            recognitionThread.IsBackground = true;
            recognitionThread.Start();
        }

        private string GetTargetPath(string filePath, string outputDirectory)
        {
            return outputDirectory + "\\" + Path.GetFileName(filePath);
        }

        private string CreateFileOutputPath(string sourceFilePath, string outputDirectoryName, bool replaceSourceFile)
        {
            string fileOutputPath = null;

            if (replaceSourceFile)
            {
                fileOutputPath = outputDirectoryName + @"\" + Path.GetFileName(sourceFilePath);   
            }
            else
            {
                fileOutputPath = outputDirectoryName + @"\" + Path.GetFileNameWithoutExtension(sourceFilePath) +
                                 FILENAMEEXTENSION_OCR + Path.GetExtension(sourceFilePath);
            }

            return fileOutputPath;
        }
        #endregion
    }
}
