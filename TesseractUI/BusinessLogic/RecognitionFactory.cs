using System;
using Clock.Pdf;
using System.IO;
using System.Threading;
using Clock.Util;

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
                string sourcePDFFile = null;
                string outputPath = null;

                file.Status = ProcessingState.Processing;
                string tesseractLanguageString = GetTesseractStringFromLanguageEnumeration(file.ProcessingLanguage);

                //using (PDFDoc doc = PDFDoc.Open(file.FilePath))
                //{
                //    doc.Ocr(OcrMode.Tesseract, tesseractLanguageString, WriteTextMode.Word, null);
                //    sourcePDFFile = doc.ReaderPDF;

                //    outputPath = CreateFileOutputPath(file.FilePath, outputDirectory, replaceSourceFile);

                //    if (File.Exists(sourcePDFFile))
                //    {
                //        File.Copy(sourcePDFFile, outputPath, true);
                //    }
                //}

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
