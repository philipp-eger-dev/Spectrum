using System;
using System.IO;
using System.Threading;
using TesseractUI.BusinessLogic;

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
                string tesseractLanguageString = GetTesseractStringFromLanguageEnumeration(file.ProcessingLanguage);

                PDFDocument document = new PDFDocument(file.FilePath);

                document.Ocr(tesseractLanguageString);

                if (this.RecognitionFinished != null)
                {
                    this.RecognitionFinished(this, new RecognitionEventArgs(file));                    
                }
            });

            recognitionThread.IsBackground = true;
            recognitionThread.Start();
        }

        private string GetTesseractStringFromLanguageEnumeration(Language language)
        {
            string tesseractLanguageString = null;

            switch (language)
            {
                case Language.English:
                    tesseractLanguageString = "eng";
                    break;
                case Language.German:
                    tesseractLanguageString = "deu";
                    break;
            }

            return tesseractLanguageString;
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
